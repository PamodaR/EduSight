using ECOMSYSTEM.Shared;
using ECOMSYSTEM.Shared.Models;
using ESCHOOLING.Shared;
using ESCHOOLING.Shared.Models;
using ESCHOOLING.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;

namespace ESCHOOLING.Web.Controllers
{
    [Authorize(Roles = "Parent")]
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
        /// The parent note service.
        /// </summary>
        private readonly IParentNoteService _parentNoteService;
        /// <summary>
        /// The events service.
        /// </summary>
        private readonly IEventsService _eventsService;
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="applicatioUserService">The applicatio user service.</param>
        /// <param name="config">The configuration.</param>
        public ParentController(ILogger<TeacherController> logger, IApplicatioUser applicatioUserService, IConfiguration config, IWebHostEnvironment webHostEnvironment, IMarksService marksService, IOnnxMarkPredictionService onnxMarkPredictionService, IStudentMarksEntryService studentMarksEntryService, IStudentBehaviourEntryService studentBehaviourEntryService, IParentNoteService parentNoteService, IEventsService eventsService)
        {
            _logger = logger;
            _applicationUserService = applicatioUserService;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            _marksService = marksService;
            _onnxMarkPredictionService = onnxMarkPredictionService;
            _studentMarksEntryService = studentMarksEntryService;
            _studentBehaviourEntryService = studentBehaviourEntryService;
            _parentNoteService = parentNoteService;
            _eventsService = eventsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ParentHome()
        {
            const int months = 6;
            var model = new ParentDashboardModel();

            var parentId = User.GetUserId();
            var parent = await _applicationUserService.GetUserByIdAsync(parentId);

            if (parent.ChildStudentId == null)
            {
                return View(model);
            }

            var childId = parent.ChildStudentId.Value;

            var attendanceRates = await _applicationUserService.GetAttendanceRateByMonthForStudentAsync(childId, months);
            for (var i = months - 1; i >= 0; i--)
            {
                var monthKey = DateTime.Today.AddMonths(-i).ToString("yyyy-MM");
                model.AttendanceTrendMonths.Add(monthKey);
                model.AttendanceTrendRates.Add(attendanceRates.GetValueOrDefault(monthKey));
            }
            model.AttendanceRate = model.AttendanceTrendRates.Count > 0 ? model.AttendanceTrendRates[^1] : 0;

            var allMarksEntries = await _studentMarksEntryService.GetAllMarksEntriesAsync();
            var latestPerSubject = allMarksEntries
                .Where(m => m.StudentId == childId)
                .GroupBy(m => m.Subject)
                .Select(g => g.OrderByDescending(m => m.CreatedDate).First())
                .ToList();

            foreach (var mark in latestPerSubject)
            {
                model.MarksSubjects.Add(mark.Subject ?? "Unknown");
                model.MarksLatestValues.Add((double)mark.Marks);
            }

            model.LatestMarksAverageDisplay = latestPerSubject.Count > 0
                ? Math.Round(latestPerSubject.Average(m => (double)m.Marks), 1).ToString(CultureInfo.InvariantCulture)
                : "No marks yet";

            var allBehaviourEntries = await _studentBehaviourEntryService.GetAllBehaviourEntriesAsync();
            var childBehaviourEntries = allBehaviourEntries.Where(b => b.StudentId == childId).ToList();
            model.BehaviourPositiveCount = childBehaviourEntries.Count(b => string.Equals(b.BehaviourType, "Positive", StringComparison.OrdinalIgnoreCase));
            model.BehaviourNegativeCount = childBehaviourEntries.Count(b => string.Equals(b.BehaviourType, "Negative", StringComparison.OrdinalIgnoreCase));
            model.BehaviourNeutralCount = childBehaviourEntries.Count(b => string.Equals(b.BehaviourType, "Neutral", StringComparison.OrdinalIgnoreCase));

            return View(model);
        }

        /// <summary>
        /// Read-only view of the logged-in parent's linked child's attendance for a given month.
        /// </summary>
        public async Task<IActionResult> ViewAttendanceDetails(string date = null)
        {
            if (date == null)
            {
                date = DateTime.Today.ToString("yyyy-MM");
            }

            ViewBag.Date = date;

            var parentId = User.GetUserId();
            var parent = await _applicationUserService.GetUserByIdAsync(parentId);

            if (parent.ChildStudentId == null)
            {
                return View(Enumerable.Empty<Attendance>());
            }

            var childAttendance = await _applicationUserService.GetAttendanceListAsync(parent.ChildStudentId.Value, date);

            return View(childAttendance ?? new List<Attendance>());
        }

        public async Task<IActionResult> ViewEvents()
        {
            var parentId = User.GetUserId();
            var parent = await _applicationUserService.GetUserByIdAsync(parentId);

            if (parent.ChildStudentId == null)
            {
                return View(Enumerable.Empty<ESCHOOLING.Shared.Models.Events>());
            }

            var child = await _applicationUserService.GetUserByIdAsync(parent.ChildStudentId.Value);

            var allEvents = await _eventsService.GetAllEventsAsync();
            var visibleEvents = allEvents
                .Where(e => e.Grade == null || e.Grade == child.Grade)
                .OrderBy(e => e.Date)
                .ToList();

            return View(visibleEvents);
        }

        /// <summary>
        /// Read-only view of the logged-in parent's linked child's real, teacher-entered marks.
        /// </summary>
        public async Task<IActionResult> ViewMarks()
        {
            var parentId = User.GetUserId();
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

            var parentId = User.GetUserId();
            var parent = await _applicationUserService.GetUserByIdAsync(parentId);

            if (parent.ChildStudentId == null)
            {
                return View(Enumerable.Empty<StudentBehaviourEntry>());
            }

            var monthEntries = await _studentBehaviourEntryService.GetBehaviourEntriesForMonthAsync(month);
            var childEntries = monthEntries.Where(e => e.StudentId == parent.ChildStudentId).ToList();

            return View(childEntries);
        }

        public IActionResult SendNote()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendNote(string noteText)
        {
            var parentId = User.GetUserId();
            var parent = await _applicationUserService.GetUserByIdAsync(parentId);

            if (parent.ChildStudentId == null)
            {
                return Json(new { success = false, message = "No child is linked to this parent account. Please contact the school office." });
            }

            var note = new ParentNote
            {
                ParentId = parentId,
                StudentId = parent.ChildStudentId.Value,
                NoteText = noteText,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            try
            {
                var result = await _parentNoteService.SaveNoteAsync(note);

                if (result.Id != 0)
                {
                    return Json(new { success = true });
                }

                _logger.LogError("SendNote: save reported no rows affected for parentId {ParentId}", parentId);
                return Json(new { success = false, message = "Save failed. No rows were written; see server logs for details." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendNote: failed to save note for parentId {ParentId}", parentId);
                return Json(new { success = false, message = $"Save failed: {ex.Message}" });
            }
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
            var parentId = User.GetUserId();
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
