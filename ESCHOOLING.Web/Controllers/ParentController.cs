using ECOMSYSTEM.Shared;
using ECOMSYSTEM.Shared.Models;
using ESCHOOLING.Shared;
using ESCHOOLING.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ESCHOOLING.Web.Controllers
{
    public class ParentController : Controller
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _config;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TeacherController> _logger;
        /// <summary>
        /// The application user service
        /// </summary>
        private readonly IApplicatioUser _applicationUserService;
        /// <summary>
        /// The marks service
        /// </summary>
        private readonly IMarksService _marksService;
        /// <summary>
        /// Gets the web host environment.
        /// </summary>
        /// <value>
        /// The web host environment.
        /// </value>
        private readonly IWebHostEnvironment _webHostEnvironment;
        /// <summary>
        /// Singleton service (registered and eagerly loaded in Program.cs) that owns the
        /// cached ONNX InferenceSession for mark prediction.
        /// </summary>
        private readonly IOnnxMarkPredictionService _onnxMarkPredictionService;
        /// <summary>
        /// The real, teacher-entered marks service. Independent of <see cref="_marksService"/>
        /// (the Predict Mark system) — the two are never mixed.
        /// </summary>
        private readonly IStudentMarksEntryService _studentMarksEntryService;
        /// <summary>
        /// The student behaviour entry service.
        /// </summary>
        private readonly IStudentBehaviourEntryService _studentBehaviourEntryService;
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="applicatioUserService">The applicatio user service.</param>
        /// <param name="config">The configuration.</param>
        public ParentController(ILogger<TeacherController> logger, IApplicatioUser applicatioUserService, IConfiguration config, IWebHostEnvironment webHostEnvironment, IMarksService marksService, IOnnxMarkPredictionService onnxMarkPredictionService, IStudentMarksEntryService studentMarksEntryService, IStudentBehaviourEntryService studentBehaviourEntryService)
        {
            _logger = logger;
            _applicationUserService = applicatioUserService;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            _marksService = marksService;
            _onnxMarkPredictionService = onnxMarkPredictionService;
            _studentMarksEntryService = studentMarksEntryService;
            _studentBehaviourEntryService = studentBehaviourEntryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ParentHome()
        {
            return View();
        }

        public IActionResult ViewAttendanceDetails()
        {
            return View();
        }

        public IActionResult ViewEvents()
        {
            return View();
        }

        /// <summary>
        /// Read-only view of the logged-in parent's linked child's real, teacher-entered marks.
        /// </summary>
        public async Task<IActionResult> ViewMarks()
        {
            var parentId = ApplicationSession.applicationUserId;
            var parent = await _applicationUserService.GetUserByIdAsync(parentId);

            if (parent.ChildStudentId == null)
            {
                return View(Enumerable.Empty<StudentMarksEntry>());
            }

            var allEntries = await _studentMarksEntryService.GetAllMarksEntriesAsync();
            var childEntries = allEntries.Where(e => e.StudentId == parent.ChildStudentId).ToList();

            return View(childEntries);
        }

        /// <summary>
        /// Read-only view of the logged-in parent's linked child's behaviour notes for a given month.
        /// </summary>
        public async Task<IActionResult> ViewBehaviourReport(string month = null)
        {
            if (month == null)
            {
                month = DateTime.Today.ToString("yyyy-MM");
            }

            ViewBag.Month = month;

            var parentId = ApplicationSession.applicationUserId;
            var parent = await _applicationUserService.GetUserByIdAsync(parentId);

            if (parent.ChildStudentId == null)
            {
                return View(Enumerable.Empty<StudentBehaviourEntry>());
            }

            var monthEntries = await _studentBehaviourEntryService.GetBehaviourEntriesForMonthAsync(month);
            var childEntries = monthEntries.Where(e => e.StudentId == parent.ChildStudentId).ToList();

            return View(childEntries);
        }

        /// <summary>
        /// Trial mark-prediction form. Parents can predict but not save — only Teacher
        /// can persist a predicted mark (TeacherController.SaveMark).
        /// </summary>
        public IActionResult PredictMark()
        {
            return View();
        }

        /// <summary>
        /// Runs the same ONNX model Teacher uses, for the logged-in parent's trial marks.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PredictMark(double mark1, double mark2, double mark3)
        {
            var parentId = ApplicationSession.applicationUserId;
            _logger.LogInformation("PredictMark (Parent): request received for parentId {ParentId}", parentId);

            try
            {
                var predictedValue = await _onnxMarkPredictionService.PredictAsync(
                    (float)mark1, (float)mark2, (float)mark3,
                    TimeSpan.FromSeconds(400), TimeSpan.FromSeconds(20),
                    HttpContext.RequestAborted);

                _logger.LogInformation("PredictMark (Parent): inference complete for parentId {ParentId}, predictedMark {PredictedMark}", parentId, predictedValue);

                return Json(new
                {
                    success = true,
                    predictedMark = Math.Round(predictedValue, 1)
                });
            }
            catch (OnnxModelLoadTimeoutException)
            {
                _logger.LogError("PredictMark (Parent): ONNX model load did not finish in time for parentId {ParentId}", parentId);
                return Json(new
                {
                    success = false,
                    message = "Prediction timed out while loading the model. Please try again."
                });
            }
            catch (OnnxInferenceTimeoutException)
            {
                _logger.LogError("PredictMark (Parent): session.Run did not finish in time for parentId {ParentId} — inference call itself is hanging", parentId);
                return Json(new
                {
                    success = false,
                    message = "Prediction timed out during inference. Please try again."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PredictMark (Parent): ONNX mark prediction failed for parentId {ParentId}. FULL EXCEPTION: {FullException}", parentId, ex.ToString());
                return Json(new
                {
                    success = false,
                    message = "Prediction failed. See server logs for details."
                });
            }
        }

    }
}
