using System;
using System.Threading;
using System.Threading.Tasks;

namespace ESCHOOLING.Shared
{
    public interface IOnnxMarkPredictionService
    {
        /// <summary>
        /// Waits for the one-time model load to finish. Called once at application startup
        /// (see Program.cs) so the app doesn't start accepting requests until the model is
        /// already warm — the first PredictMark call of the day is then just as fast as any other.
        /// </summary>
        Task WarmupAsync();

        /// <summary>
        /// Runs the mark-prediction model against the three input marks, reusing the
        /// singleton InferenceSession loaded once for the lifetime of the process.
        /// </summary>
        Task<float> PredictAsync(float mark1, float mark2, float mark3, TimeSpan modelLoadTimeout, TimeSpan inferenceTimeout, CancellationToken cancellationToken);
    }

    /// <summary>Thrown when the ONNX model hasn't finished its (one-time) load within the requested timeout.</summary>
    public class OnnxModelLoadTimeoutException : Exception
    {
        public OnnxModelLoadTimeoutException() : base("ONNX model load did not complete within the allotted timeout.") { }
    }

    /// <summary>Thrown when a call to InferenceSession.Run doesn't return within the requested timeout.</summary>
    public class OnnxInferenceTimeoutException : Exception
    {
        public OnnxInferenceTimeoutException() : base("ONNX inference did not complete within the allotted timeout.") { }
    }
}
