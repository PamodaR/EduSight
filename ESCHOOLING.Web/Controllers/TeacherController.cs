using ECOMSYSTEM.Shared;
using ECOMSYSTEM.Shared.Models;
using ECOMSYSTEM.Web.Services;
using ESCHOOLING.Shared;
using ESCHOOLING.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="applicatioUserService">The applicatio user service.</param>
        /// <param name="config">The configuration.</param>
        public TeacherController(ILogger<TeacherController> logger, IApplicatioUser applicatioUserService, IConfiguration config, IWebHostEnvironment webHostEnvironment, IMarksService marksService)
        {
            _logger = logger;
            _applicationUserService = applicatioUserService;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            _marksService = marksService;
        }

        public async Task<IActionResult> TeacherHome()
        {
            TeacherDashboadModel teacherDashboadModel = new TeacherDashboadModel();
            teacherDashboadModel.StudentCount = 0;
            teacherDashboadModel.ParentCount = 0;
            teacherDashboadModel.AdmintCount = 0;
            teacherDashboadModel.TeacherCount = 0;

            var counts = await _applicationUserService.GetUserCountsByTypeAsync();
            teacherDashboadModel.AdmintCount = counts.GetValueOrDefault(0);
            teacherDashboadModel.TeacherCount = counts.GetValueOrDefault(1);
            teacherDashboadModel.StudentCount = counts.GetValueOrDefault(2);
            teacherDashboadModel.ParentCount = counts.Where(c => c.Key != 0 && c.Key != 1 && c.Key != 2).Sum(c => c.Value);

            return View(teacherDashboadModel);
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
            return View("ViewAttendanceDetails", results);
        }

        public async Task<IActionResult> ViewAttendanceDetails(long id, string date = null)
        {
            if(date == null)
            {
                date = DateTime.Today.ToString(("yyyy-MM"));
            }

            var results = await _applicationUserService.GetAttendanceListAsync(id, date);
            return View("ViewAttendanceDetails", results);
        }

        public IActionResult MarkPrediction()
        {
            return View();
        }

        /// <summary>
        /// Placeholder prediction endpoint. Returns the average of the three prior test marks
        /// as a stand-in result until the trained ML model is wired in to replace this.
        /// </summary>
        [HttpPost]
        public IActionResult PredictMark(long studentId, string subject, double mark1, double mark2, double mark3)
        {
            var placeholderPrediction = Math.Round((mark1 + mark2 + mark3) / 3, 1);

            return Json(new
            {
                success = true,
                predictedMark = placeholderPrediction
            });
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

            var result = await _marksService.SaveMarkAsync(markObject);

            if (result.Id != 0)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
    }
}
