using ECOMSYSTEM.Shared;
using ECOMSYSTEM.Shared.Models;
using ESCHOOLING.Shared;
using ESCHOOLING.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;

namespace ESCHOOLING.Web.Controllers
{
    public class StudentController : Controller
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
        /// The homework service
        /// </summary>
        private readonly IHomeworkService _homeworkService;
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
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="applicatioUserService">The applicatio user service.</param>
        /// <param name="config">The configuration.</param>
        public StudentController(ILogger<TeacherController> logger, IApplicatioUser applicatioUserService, IConfiguration config, IWebHostEnvironment webHostEnvironment, IMarksService marksService, IHomeworkService homeworkService, IOnnxMarkPredictionService onnxMarkPredictionService, IStudentMarksEntryService studentMarksEntryService)
        {
            _logger = logger;
            _applicationUserService = applicatioUserService;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            _marksService = marksService;
            _homeworkService = homeworkService;
            _onnxMarkPredictionService = onnxMarkPredictionService;
            _studentMarksEntryService = studentMarksEntryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> StudentHome()
        {
            const int months = 6;
            var studentId = ApplicationSession.applicationUserId;
            var model = new StudentDashboardModel();

            var attendanceRates = await _applicationUserService.GetAttendanceRateByMonthForStudentAsync(studentId, months);
            for (var i = months - 1; i >= 0; i--)
            {
                var monthKey = DateTime.Today.AddMonths(-i).ToString("yyyy-MM");
                model.AttendanceTrendMonths.Add(monthKey);
                model.AttendanceTrendRates.Add(attendanceRates.GetValueOrDefault(monthKey));
            }
            model.AttendanceRate = model.AttendanceTrendRates.Count > 0 ? model.AttendanceTrendRates[^1] : 0;

            var allMarks = await _marksService.GetAllMarksAsync();
            var latestPerSubject = allMarks
                .Where(m => m.StudentId == studentId)
                .GroupBy(m => m.Subject)
                .Select(g => g.OrderByDescending(m => m.CreatedDate).First())
                .ToList();

            var parsedValues = new List<double>();
            foreach (var mark in latestPerSubject)
            {
                if (!double.TryParse(mark.PredictedMark, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                {
                    continue;
                }

                model.MarksSubjects.Add(mark.Subject ?? "Unknown");
                model.MarksLatestValues.Add(value);
                parsedValues.Add(value);
            }

            model.LatestMarksAverageDisplay = parsedValues.Count > 0
                ? Math.Round(parsedValues.Average(), 1).ToString(CultureInfo.InvariantCulture)
                : "No marks yet";

            return View(model);
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
        /// Read-only view of the student's own real, teacher-entered marks.
        /// </summary>
        public async Task<IActionResult> ViewMarks()
        {
            var studentId = ApplicationSession.applicationUserId;
            var allEntries = await _studentMarksEntryService.GetAllMarksEntriesAsync();
            var studentEntries = allEntries.Where(e => e.StudentId == studentId).ToList();

            return View(studentEntries);
        }

        /// <summary>
        /// Read-only view of the student's own predicted (and saved) marks.
        /// </summary>
        public async Task<IActionResult> PredictedMarks()
        {
            var studentId = ApplicationSession.applicationUserId;
            var allMarks = await _marksService.GetAllMarksAsync();
            var studentMarks = allMarks.Where(m => m.StudentId == studentId).ToList();

            return View(studentMarks);
        }

        /// <summary>
        /// Trial mark-prediction form. Students can predict but not save — only Teacher
        /// can persist a predicted mark (TeacherController.SaveMark).
        /// </summary>
        public IActionResult PredictMark()
        {
            return View();
        }

        /// <summary>
        /// Runs the same ONNX model Teacher uses, for the logged-in student's own trial marks.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PredictMark(double mark1, double mark2, double mark3)
        {
            var studentId = ApplicationSession.applicationUserId;
            _logger.LogInformation("PredictMark (Student): request received for studentId {StudentId}", studentId);

            try
            {
                var predictedValue = await _onnxMarkPredictionService.PredictAsync(
                    (float)mark1, (float)mark2, (float)mark3,
                    TimeSpan.FromSeconds(400), TimeSpan.FromSeconds(20),
                    HttpContext.RequestAborted);

                _logger.LogInformation("PredictMark (Student): inference complete for studentId {StudentId}, predictedMark {PredictedMark}", studentId, predictedValue);

                return Json(new
                {
                    success = true,
                    predictedMark = Math.Round(predictedValue, 1)
                });
            }
            catch (OnnxModelLoadTimeoutException)
            {
                _logger.LogError("PredictMark (Student): ONNX model load did not finish in time for studentId {StudentId}", studentId);
                return Json(new
                {
                    success = false,
                    message = "Prediction timed out while loading the model. Please try again."
                });
            }
            catch (OnnxInferenceTimeoutException)
            {
                _logger.LogError("PredictMark (Student): session.Run did not finish in time for studentId {StudentId} — inference call itself is hanging", studentId);
                return Json(new
                {
                    success = false,
                    message = "Prediction timed out during inference. Please try again."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PredictMark (Student): ONNX mark prediction failed for studentId {StudentId}. FULL EXCEPTION: {FullException}", studentId, ex.ToString());
                return Json(new
                {
                    success = false,
                    message = "Prediction failed. See server logs for details."
                });
            }
        }

        /// <summary>
        /// Lists active homework posted for the logged-in student's own grade.
        /// </summary>
        public async Task<IActionResult> ViewHomework()
        {
            var studentId = ApplicationSession.applicationUserId;
            var student = await _applicationUserService.GetUserByIdAsync(studentId);

            var allHomework = await _homeworkService.GetAllHomeworkAsync();
            var gradeHomework = allHomework
                .Where(h => h.Grade == student.Grade)
                .OrderByDescending(h => h.DueDate)
                .ToList();

            return View(gradeHomework);
        }

    }
}
