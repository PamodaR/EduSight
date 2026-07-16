using ECOMSYSTEM.Shared;
using ECOMSYSTEM.Shared.Enum;
using ECOMSYSTEM.Shared.Models;
using ECOMSYSTEM.Web.Services;
using ESCHOOLING.Shared;
using ESCHOOLING.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ESCHOOLING.Web.Controllers
{
    public class TeacherController : Controller
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
        /// Gets the web host environment.
        /// </summary>
        /// <value>
        /// The web host environment.
        /// </value>
        private readonly IWebHostEnvironment _webHostEnvironment;
        /// <summary>
        /// The marks service
        /// </summary>
        private readonly IMarksService _marksService;
        /// <summary>
        /// The homework service
        /// </summary>
        private readonly IHomeworkService _homeworkService;
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
        /// The counselor service (existing Admin-side counselor directory).
        /// </summary>
        private readonly ICounselorService _counselorService;
        /// <summary>
        /// The counselling referral service.
        /// </summary>
        private readonly ICounsellingReferralService _counsellingReferralService;
        /// <summary>
        /// The email service, used to best-effort notify a counselor of a new referral.
        /// </summary>
        private readonly IEmailService _emailService;
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="applicatioUserService">The applicatio user service.</param>
        /// <param name="config">The configuration.</param>
        public TeacherController(ILogger<TeacherController> logger, IApplicatioUser applicatioUserService, IConfiguration config, IWebHostEnvironment webHostEnvironment, IMarksService marksService, IHomeworkService homeworkService, IOnnxMarkPredictionService onnxMarkPredictionService, IStudentMarksEntryService studentMarksEntryService, IStudentBehaviourEntryService studentBehaviourEntryService, IParentNoteService parentNoteService, ICounselorService counselorService, ICounsellingReferralService counsellingReferralService, IEmailService emailService)
        {
            _logger = logger;
            _applicationUserService = applicatioUserService;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            _marksService = marksService;
            _homeworkService = homeworkService;
            _onnxMarkPredictionService = onnxMarkPredictionService;
            _studentMarksEntryService = studentMarksEntryService;
            _studentBehaviourEntryService = studentBehaviourEntryService;
            _parentNoteService = parentNoteService;
            _counselorService = counselorService;
            _counsellingReferralService = counsellingReferralService;
            _emailService = emailService;
        }

        public async Task<IActionResult> TeacherHome()
        {
            const int months = 6;
            var model = new TeacherDashboardModel();

            var students = await _applicationUserService.GetUsersExcludingTypesAsync(0, 1, 3);
            model.TotalStudentsManaged = students.Count;

            var (present, totalMarked) = await _applicationUserService.GetTodayAttendanceStatusAsync();
            model.TodayPresentCount = present;
            model.TodayTotalMarkedCount = totalMarked;
            model.TodayAttendanceRate = totalMarked == 0 ? 0 : Math.Round(present * 100.0 / totalMarked, 1);

            var attendanceRates = await _applicationUserService.GetAttendanceRateByMonthAsync(months);
            for (var i = months - 1; i >= 0; i--)
            {
                var monthKey = DateTime.Today.AddMonths(-i).ToString("yyyy-MM");
                model.AttendanceTrendMonths.Add(monthKey);
                model.AttendanceTrendRates.Add(attendanceRates.GetValueOrDefault(monthKey));
            }

            var buckets = new[] { "A", "B", "C", "D", "F" };
            var bucketCounts = buckets.ToDictionary(b => b, b => 0);
            var allMarks = await _marksService.GetAllMarksAsync();
            foreach (var mark in allMarks)
            {
                if (!double.TryParse(mark.PredictedMark, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                {
                    continue;
                }

                var bucket = value >= 90 ? "A" : value >= 80 ? "B" : value >= 70 ? "C" : value >= 60 ? "D" : "F";
                bucketCounts[bucket]++;
            }
            model.MarksDistributionBuckets.AddRange(buckets);
            model.MarksDistributionCounts.AddRange(buckets.Select(b => bucketCounts[b]));

            return View(model);
        }

        public IActionResult AddStudent()
        {
            return View();
        }

        public async Task<IActionResult> SaveStudent(ApplicationUser studentInfo)
        {
            var result = await _applicationUserService.RegisterUserAsync(studentInfo);

            if (result.UserId != 0 && result.Email != null)
            {
                return Json(new
                {
                    success = true
                });
            }

            return Json(new
            {
                success = false,
            });
        }

        /// <summary>
        /// Views the user list.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewStudentList()
        {
            var results = await _applicationUserService.GetUsersExcludingTypesAsync(0, 1, 3);
            return View(results);
        }

        [HttpGet]
        public async Task<IActionResult> EditStudent(long id)
        {
            var user = await _applicationUserService.GetUserByIdAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditStudent(ApplicationUser userInfo)
        {
            var result = await _applicationUserService.UpdateUserAsync(userInfo);
            TempData["Message"] = (result.UserId != 0) ? "Updated successfully." : "Update failed.";
            return RedirectToAction(nameof(ViewStudentList));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            var result = await _applicationUserService.DeactivateUserAsync(id);
            TempData["Message"] = result ? "Deactivated successfully." : "Deactivation failed.";
            return RedirectToAction(nameof(ViewStudentList));
        }

        /// <summary>
        /// Views the user list.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> MarkAttendance()
        {
            var results = await _applicationUserService.GetUsersExcludingTypesAsync(0, 1, 3);
            return View(results);
        }

        public async Task<IActionResult> SearchForAttendance(int grade)
        {
            var results = await _applicationUserService.SearchAsync(grade);
            return View("MarkAttendance", results);
        }

        public async Task<IActionResult> SearchForStudents(int grade)
        {
            var results = await _applicationUserService.SearchAsync(grade);
            return View("ViewStudentList", results);
        }

        public async Task<IActionResult> UpdateAttendance(Attendance attendanceObj)
        {
            var result = await _applicationUserService.UpdateAttendanceAsync(attendanceObj);
            if(result != null)
            {
                return Json(new
                {
                    success = true,
                    response = result
                });
            }
            return Json(new
            {
                success = false
            });
        }

        public async Task<IActionResult> ViewAttendanceList()
        {
            var results = await _applicationUserService.GetUsersExcludingTypesAsync(0, 1, 3);
            return View(results);
        }

        public async Task<IActionResult> SearchForStudentsId(long id)
        {
            var results = await _applicationUserService.SearchByIdAsync(id);
            return View("ViewAttendanceList", results);
        }

        public async Task<IActionResult> SearchForMonth(string date)
        {
            var results = await _applicationUserService.SearchForMonthAsync(date);
            ViewBag.StudentId = 0L;
            ViewBag.Date = date;
            return View("ViewAttendanceDetails", results);
        }

        public async Task<IActionResult> ViewAttendanceDetails(long id, string date = null)
        {
            if(date == null)
            {
                date = DateTime.Today.ToString(("yyyy-MM"));
            }

            var results = await _applicationUserService.GetAttendanceListAsync(id, date);
            ViewBag.StudentId = id;
            ViewBag.Date = date;
            return View("ViewAttendanceDetails", results);
        }

        public async Task<IActionResult> ExportToExcel(long id, string date = null)
        {
            if (date == null)
            {
                date = DateTime.Today.ToString("yyyy-MM");
            }

            var results = id > 0
                ? await _applicationUserService.GetAttendanceListAsync(id, date)
                : await _applicationUserService.SearchForMonthAsync(date);

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Attendance");

            worksheet.Cells[1, 1].Value = "Student Id";
            worksheet.Cells[1, 2].Value = "Attendance";
            worksheet.Cells[1, 3].Value = "Status";
            worksheet.Cells[1, 4].Value = "Date";

            var row = 2;
            foreach (var item in results)
            {
                worksheet.Cells[row, 1].Value = item.StudentId;
                worksheet.Cells[row, 2].Value = item.IsPresent == true ? "Present" : "Absent";
                worksheet.Cells[row, 3].Value = item.IsActive == true ? "Active" : "Not Active";
                worksheet.Cells[row, 4].Value = item.Date?.ToString("yyyy-MM-dd") ?? "";
                row++;
            }
            worksheet.Cells.AutoFitColumns();

            var stream = new MemoryStream();
            await package.SaveAsAsync(stream);
            stream.Position = 0;

            var fileName = id > 0 ? $"Attendance_Student{id}_{date}.xlsx" : $"Attendance_{date}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        /// <summary>
        /// Combined Attendance + Behaviour report for all students, for a single selected month.
        /// </summary>
        public async Task<IActionResult> MonthlyReports(string date = null)
        {
            if (date == null)
            {
                date = DateTime.Today.ToString("yyyy-MM");
            }

            var model = new MonthlyReportsViewModel
            {
                SelectedMonth = date,
                AttendanceEntries = await _applicationUserService.SearchForMonthAsync(date) ?? new List<Attendance>(),
                BehaviourEntries = await _studentBehaviourEntryService.GetBehaviourEntriesForMonthAsync(date)
            };

            return View(model);
        }

        public async Task<IActionResult> ExportBehaviourToExcel(string date = null)
        {
            if (date == null)
            {
                date = DateTime.Today.ToString("yyyy-MM");
            }

            var results = await _studentBehaviourEntryService.GetBehaviourEntriesForMonthAsync(date);

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Behaviour");

            worksheet.Cells[1, 1].Value = "Student Id";
            worksheet.Cells[1, 2].Value = "Student Name";
            worksheet.Cells[1, 3].Value = "Behaviour Type";
            worksheet.Cells[1, 4].Value = "Description";
            worksheet.Cells[1, 5].Value = "Date";

            var row = 2;
            foreach (var item in results)
            {
                worksheet.Cells[row, 1].Value = item.StudentId;
                worksheet.Cells[row, 2].Value = item.StudentName;
                worksheet.Cells[row, 3].Value = item.BehaviourType;
                worksheet.Cells[row, 4].Value = item.Description;
                worksheet.Cells[row, 5].Value = item.CreatedDate?.ToString("yyyy-MM-dd") ?? "";
                row++;
            }
            worksheet.Cells.AutoFitColumns();

            var stream = new MemoryStream();
            await package.SaveAsAsync(stream);
            stream.Position = 0;

            var fileName = $"Behaviour_{date}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        public IActionResult MarkPrediction()
        {
            return View();
        }

        /// <summary>
        /// Runs the trained ONNX regression model (ESCHOOLING.Web/MLModels/mark_prediction_model_v3.onnx)
        /// against the three input marks and returns the predicted overall score.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PredictMark(long studentId, string subject, double mark1, double mark2, double mark3)
        {
            _logger.LogInformation("PredictMark: request received for studentId {StudentId}, subject {Subject}", studentId, subject);

            try
            {
                // The one-time model load has been observed taking ~5.5 minutes; this timeout
                // is a safety net so a slow-but-in-progress load doesn't fail early, not the
                // primary fix (the primary fix is the singleton itself, loaded once at startup).
                var predictedValue = await _onnxMarkPredictionService.PredictAsync(
                    (float)mark1, (float)mark2, (float)mark3,
                    TimeSpan.FromSeconds(400), TimeSpan.FromSeconds(20),
                    HttpContext.RequestAborted);

                _logger.LogInformation("PredictMark: inference complete for studentId {StudentId}, predictedMark {PredictedMark}", studentId, predictedValue);

                return Json(new
                {
                    success = true,
                    predictedMark = Math.Round(predictedValue, 1)
                });
            }
            catch (OnnxModelLoadTimeoutException)
            {
                _logger.LogError("PredictMark: ONNX model load did not finish in time for studentId {StudentId}, subject {Subject}", studentId, subject);
                return Json(new
                {
                    success = false,
                    message = "Prediction timed out while loading the model. Please try again."
                });
            }
            catch (OnnxInferenceTimeoutException)
            {
                _logger.LogError("PredictMark: session.Run did not finish in time for studentId {StudentId}, subject {Subject} — inference call itself is hanging", studentId, subject);
                return Json(new
                {
                    success = false,
                    message = "Prediction timed out during inference. Please try again."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PredictMark: ONNX mark prediction failed for studentId {StudentId}, subject {Subject}. FULL EXCEPTION: {FullException}", studentId, subject, ex.ToString());
                return Json(new
                {
                    success = false,
                    message = "Prediction failed. See server logs for details."
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveMark(long studentId, string subject, string predictedMark)
        {
            var markObject = new Marks
            {
                StudentId = studentId,
                Subject = subject,
                PredictedMark = predictedMark,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            try
            {
                var result = await _marksService.SaveMarkAsync(markObject);

                if (result.Id != 0)
                {
                    return Json(new { success = true });
                }

                _logger.LogError("SaveMark: save reported no rows affected for studentId {StudentId}, subject {Subject}", studentId, subject);
                return Json(new { success = false, message = "Save failed. No rows were written; see server logs for details." });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 547 })
            {
                _logger.LogError(ex, "SaveMark: foreign key violation for studentId {StudentId}, subject {Subject}", studentId, subject);
                return Json(new { success = false, message = $"Save failed: student ID {studentId} does not exist." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SaveMark: failed to save mark for studentId {StudentId}, subject {Subject}", studentId, subject);
                return Json(new { success = false, message = $"Save failed: {ex.Message}" });
            }
        }

        /// <summary>
        /// Real, manually entered marks — independent of the Predict Mark / ONNX system above.
        /// </summary>
        public async Task<IActionResult> EnterMarks()
        {
            ViewBag.Students = await _applicationUserService.GetUsersByTypeAsync((int)RoleEnums.Student);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EnterMarks(long studentId, string term, string subject, decimal marks)
        {
            var entry = new StudentMarksEntry
            {
                StudentId = studentId,
                Term = term,
                Subject = subject,
                Marks = marks,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            try
            {
                var result = await _studentMarksEntryService.SaveMarksEntryAsync(entry);

                if (result.Id != 0)
                {
                    return Json(new { success = true });
                }

                _logger.LogError("EnterMarks: save reported no rows affected for studentId {StudentId}, term {Term}, subject {Subject}", studentId, term, subject);
                return Json(new { success = false, message = "Save failed. No rows were written; see server logs for details." });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 547 })
            {
                _logger.LogError(ex, "EnterMarks: foreign key violation for studentId {StudentId}", studentId);
                return Json(new { success = false, message = $"Save failed: student ID {studentId} does not exist." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EnterMarks: failed to save marks entry for studentId {StudentId}, term {Term}, subject {Subject}", studentId, term, subject);
                return Json(new { success = false, message = $"Save failed: {ex.Message}" });
            }
        }

        /// <summary>
        /// Teacher-wide view of every real marks entry recorded so far.
        /// </summary>
        public async Task<IActionResult> ViewMarksEntries()
        {
            var entries = await _studentMarksEntryService.GetAllMarksEntriesAsync();
            return View(entries);
        }

        public async Task<IActionResult> EnterBehaviour()
        {
            ViewBag.Students = await _applicationUserService.GetUsersByTypeAsync((int)RoleEnums.Student);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EnterBehaviour(long studentId, string behaviourType, string description, DateTime date)
        {
            var entry = new StudentBehaviourEntry
            {
                StudentId = studentId,
                BehaviourType = behaviourType,
                Description = description,
                MonthForSearch = date.ToString("yyyy-MM"),
                IsActive = true,
                CreatedDate = date
            };

            try
            {
                var result = await _studentBehaviourEntryService.SaveBehaviourEntryAsync(entry);

                if (result.Id != 0)
                {
                    return Json(new { success = true });
                }

                _logger.LogError("EnterBehaviour: save reported no rows affected for studentId {StudentId}, behaviourType {BehaviourType}", studentId, behaviourType);
                return Json(new { success = false, message = "Save failed. No rows were written; see server logs for details." });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 547 })
            {
                _logger.LogError(ex, "EnterBehaviour: foreign key violation for studentId {StudentId}", studentId);
                return Json(new { success = false, message = $"Save failed: student ID {studentId} does not exist." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EnterBehaviour: failed to save behaviour entry for studentId {StudentId}, behaviourType {BehaviourType}", studentId, behaviourType);
                return Json(new { success = false, message = $"Save failed: {ex.Message}" });
            }
        }

        /// <summary>
        /// Teacher-wide view of every behaviour entry recorded so far.
        /// </summary>
        public async Task<IActionResult> ViewBehaviourEntries()
        {
            var entries = await _studentBehaviourEntryService.GetAllBehaviourEntriesAsync();
            return View(entries);
        }

        /// <summary>
        /// Teacher-wide view of every note parents have sent about their children.
        /// </summary>
        public async Task<IActionResult> ViewParentNotes()
        {
            var notes = await _parentNoteService.GetAllNotesAsync();
            return View(notes);
        }

        public async Task<IActionResult> ReferToCounselling()
        {
            ViewBag.Students = await _applicationUserService.GetUsersByTypeAsync((int)RoleEnums.Student);
            ViewBag.Counselors = await _counselorService.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReferToCounselling(long studentId, long counselorId, string reason)
        {
            var teacherId = ApplicationSession.applicationUserId;

            var referral = new CounsellingReferral
            {
                StudentId = studentId,
                CounselorId = counselorId,
                TeacherId = teacherId,
                Reason = reason,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            try
            {
                var result = await _counsellingReferralService.SaveReferralAsync(referral);

                if (result.Id == 0)
                {
                    _logger.LogError("ReferToCounselling: save reported no rows affected for studentId {StudentId}, counselorId {CounselorId}", studentId, counselorId);
                    return Json(new { success = false, message = "Save failed. No rows were written; see server logs for details." });
                }
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 547 })
            {
                _logger.LogError(ex, "ReferToCounselling: foreign key violation for studentId {StudentId}, counselorId {CounselorId}", studentId, counselorId);
                return Json(new { success = false, message = "Save failed: selected student or counselor does not exist." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReferToCounselling: failed to save referral for studentId {StudentId}, counselorId {CounselorId}", studentId, counselorId);
                return Json(new { success = false, message = $"Save failed: {ex.Message}" });
            }

            // The referral is saved at this point. The email notification is best-effort —
            // a failure here must not make it look like the referral itself was lost.
            try
            {
                var counselor = await _counselorService.GetByIdAsync(counselorId);
                var student = await _applicationUserService.GetUserByIdAsync(studentId);
                var teacher = await _applicationUserService.GetUserByIdAsync(teacherId);

                var subject = $"Student Counselling Referral: {student?.Username}";
                var body = $"Dear {counselor?.Name},\n\n" +
                           $"{teacher?.Username} has referred {student?.Username} for counselling.\n\n" +
                           $"Reason: {reason}\n\n" +
                           "Please reach out to the school to arrange a session.\n\n" +
                           "— EduSight";

                await _emailService.SendEmailAsync(counselor!.Email!, counselor.Name!, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReferToCounselling: referral saved but email notification failed for counselorId {CounselorId}", counselorId);
                return Json(new { success = true, emailSent = false, message = "Referral saved, but the email notification to the counselor could not be sent." });
            }

            return Json(new { success = true, emailSent = true });
        }

        /// <summary>
        /// Teacher-wide view of every counselling referral recorded so far.
        /// </summary>
        public async Task<IActionResult> ViewReferrals()
        {
            var referrals = await _counsellingReferralService.GetAllReferralsAsync();
            return View(referrals);
        }

        public IActionResult AddHomework()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveHomework(int grade, string subject, string description, DateTime dueDate)
        {
            var homeworkObject = new Homework
            {
                TeacherId = ApplicationSession.applicationUserId,
                Grade = grade,
                Subject = subject,
                Description = description,
                DueDate = dueDate,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            var result = await _homeworkService.SaveHomeworkAsync(homeworkObject);

            if (result.Id != 0)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
    }
}
