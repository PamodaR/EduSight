using ESCHOOLING.Shared;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Web.Services
{
    /// <summary>
    /// Registered as a singleton, constructed eagerly at startup (see Program.cs), which starts
    /// loading the ONNX model immediately. Program.cs also awaits <see cref="WarmupAsync"/>
    /// before the app starts accepting requests, so the model is fully loaded before anyone can
    /// hit PredictMark — not just kicked off in the background on first use. The InferenceSession
    /// is loaded exactly once per process and reused for every subsequent prediction.
    /// </summary>
    public class OnnxMarkPredictionService : IOnnxMarkPredictionService, IDisposable
    {
        private readonly ILogger<OnnxMarkPredictionService> _logger;
        private readonly Task<InferenceSession> _sessionTask;

        public OnnxMarkPredictionService(IWebHostEnvironment webHostEnvironment, ILogger<OnnxMarkPredictionService> logger)
        {
            _logger = logger;
            var modelPath = Path.Combine(webHostEnvironment.ContentRootPath, "MLModels", "mark_prediction_model_v3.onnx");

            _sessionTask = Task.Run(() =>
            {
                if (!File.Exists(modelPath))
                {
                    throw new FileNotFoundException($"ONNX model not found at '{modelPath}'.", modelPath);
                }

                var stopwatch = Stopwatch.StartNew();
                var session = new InferenceSession(modelPath);
                _logger.LogInformation("ONNX prediction model loaded in {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
                return session;
            });
        }

        public async Task WarmupAsync()
        {
            await _sessionTask;
        }

        public async Task<float> PredictAsync(float mark1, float mark2, float mark3, TimeSpan modelLoadTimeout, TimeSpan inferenceTimeout, CancellationToken cancellationToken)
        {
            InferenceSession session;
            try
            {
                session = await _sessionTask.WaitAsync(modelLoadTimeout, cancellationToken);
            }
            catch (TimeoutException)
            {
                throw new OnnxModelLoadTimeoutException();
            }

            var inferenceTask = Task.Run(() =>
            {
                try
                {
                    var inputTensor = new DenseTensor<float>(new[] { 1, 3 });
                    inputTensor[0, 0] = mark1;
                    inputTensor[0, 1] = mark2;
                    inputTensor[0, 2] = mark3;

                    var inputs = new List<NamedOnnxValue>
                    {
                        NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
                    };

                    _logger.LogInformation("OnnxMarkPredictionService: >>> calling session.Run (mark1={Mark1}, mark2={Mark2}, mark3={Mark3})", mark1, mark2, mark3);
                    using var results = session.Run(inputs);
                    _logger.LogInformation("OnnxMarkPredictionService: <<< session.Run returned");
                    return results.First().AsEnumerable<float>().First();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "OnnxMarkPredictionService: session.Run threw. FULL EXCEPTION: {FullException}", ex.ToString());
                    throw;
                }
            }, cancellationToken);

            try
            {
                return await inferenceTask.WaitAsync(inferenceTimeout, cancellationToken);
            }
            catch (TimeoutException)
            {
                throw new OnnxInferenceTimeoutException();
            }
        }

        public void Dispose()
        {
            if (_sessionTask.IsCompletedSuccessfully)
            {
                _sessionTask.Result.Dispose();
            }
        }
    }
}
