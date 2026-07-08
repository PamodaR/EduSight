using ECOMSYSTEM.Shared;
using ECOMSYSTEM.Shared.Models;
using ECOMSYSTEM.Web.Services;
using ESCHOOLING.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.Http.Headers;
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
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="applicatioUserService">The applicatio user service.</param>
        /// <param name="config">The configuration.</param>
        public TeacherController(ILogger<TeacherController> logger, IApplicatioUser applicatioUserService, IConfiguration config, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _applicationUserService = applicatioUserService;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult TeacherHome()
        {
            TeacherDashboadModel teacherDashboadModel = new TeacherDashboadModel();
            teacherDashboadModel.StudentCount = 0;
            teacherDashboadModel.ParentCount = 0;
            teacherDashboadModel.AdmintCount = 0;
            teacherDashboadModel.TeacherCount = 0;

            var users = _applicationUserService.GetAllUsers();
            if (users.Count > 0)
            {
                foreach (var user in users)
                {
                    if (user.UserType == 0)
                    {
                        teacherDashboadModel.AdmintCount++;
                    }
                    else if (user.UserType == 1)
                    {
                        teacherDashboadModel.TeacherCount++;
                    }
                    else if (user.UserType == 2)
                    {
                        teacherDashboadModel.StudentCount++;
                    }
                    else
                    {
                        teacherDashboadModel.ParentCount++;
                    }
                }
            }

            return View(teacherDashboadModel);
        }

        public IActionResult AddStudent()
        {
            return View();
        }

        public IActionResult SaveStudent(ApplicationUser studentInfo)
        {
            var result = _applicationUserService.RegisterUser(studentInfo);

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
        public IActionResult ViewStudentList()
        {
            var results = _applicationUserService.GetAllUsers();
            results.RemoveAll(user => user.UserType == 0 || user.UserType == 1 || user.UserType == 3);
            return View(results);
        }

        /// <summary>
        /// Views the user list.
        /// </summary>
        /// <returns></returns>
        public IActionResult MarkAttendance()
        {
            var results = _applicationUserService.GetAllUsers();
            results.RemoveAll(user => user.UserType == 0 || user.UserType == 1 || user.UserType == 3);
            return View(results);
        }

        public IActionResult SearchForAttendance(int grade)
        {
            var results = _applicationUserService.Search(grade);
            return View("MarkAttendance", results);
        }

        public IActionResult SearchForStudents(int grade)
        {
            var results = _applicationUserService.Search(grade);
            return View("ViewStudentList", results);
        }

        public IActionResult UpdateAttendance(Attendance attendanceObj)
        {
            var result = _applicationUserService.UpdateAttendance(attendanceObj);
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

        public IActionResult ViewAttendanceList()
        {
            var results = _applicationUserService.GetAllUsers();
            results.RemoveAll(user => user.UserType == 0 || user.UserType == 1 || user.UserType == 3);
            return View(results);
        }

        public IActionResult SearchForStudentsId(long id)
        {
            var results = _applicationUserService.SearchById(id);
            return View("ViewAttendanceList", results);
        }

        public IActionResult SearchForMonth(string date)
        {
            var results = _applicationUserService.SearchForMonth(date);
            return View("ViewAttendanceDetails", results);
        }

        public IActionResult ViewAttendanceDetails(long id, string date = null)
        {
            if(date == null)
            {
                date = DateTime.Today.ToString(("yyyy-MM"));
            }
             
            var results = _applicationUserService.GetAttendanceList(id, date);
            return View("ViewAttendanceDetails", results);
        }

        public IActionResult MarkPrediction()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PredictMark(int studyHours)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsJsonAsync("/predict", new { StudyHours = studyHours });
                if (response.IsSuccessStatusCode)
                {
                    var predictedMark = await response.Content.ReadAsStringAsync();
                    ViewBag.PredictedMark = predictedMark;
                }
            }

            return View("MarkPrediction");
        }
    }
}
