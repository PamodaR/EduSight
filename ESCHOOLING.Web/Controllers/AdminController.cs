using ECOMSYSTEM.Shared;
using ECOMSYSTEM.Shared.Enum;
using ECOMSYSTEM.Shared.Models;
using ESCHOOLING.Shared;
using ESCHOOLING.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ESCHOOLING.Web.Controllers
{
    public class AdminController : Controller
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _config;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<AdminController> _logger;
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
        /// The counselor service
        /// </summary>
        private readonly ICounselorService _counselorService;
        /// <summary>
        /// The events service
        /// </summary>
        private readonly IEventsService _eventsService;
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="applicatioUserService">The applicatio user service.</param>
        /// <param name="config">The configuration.</param>
        public AdminController(ILogger<AdminController> logger, IApplicatioUser applicatioUserService, IConfiguration config, IWebHostEnvironment webHostEnvironment, ICounselorService counselorService, IEventsService eventsService)
        {
            _logger = logger;
            _applicationUserService = applicatioUserService;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            _counselorService = counselorService;
            _eventsService = eventsService;
        }

        public async Task<IActionResult> AdminHome()
        {
            const int months = 6;
            var model = new AdminDashboardModel();

            var counts = await _applicationUserService.GetUserCountsByTypeAsync();
            model.AdminCount = counts.GetValueOrDefault(0);
            model.TeacherCount = counts.GetValueOrDefault(1);
            model.StudentCount = counts.GetValueOrDefault(2);
            model.ParentCount = counts.Where(c => c.Key != 0 && c.Key != 1 && c.Key != 2).Sum(c => c.Value);
            model.CounselorCount = await _counselorService.GetActiveCounselorCountAsync();

            var registrationCounts = await _applicationUserService.GetRegistrationCountsByMonthAsync(months);
            var attendanceRates = await _applicationUserService.GetAttendanceRateByMonthAsync(months);

            for (var i = months - 1; i >= 0; i--)
            {
                var monthKey = DateTime.Today.AddMonths(-i).ToString("yyyy-MM");
                model.RegistrationMonths.Add(monthKey);
                model.RegistrationCounts.Add(registrationCounts.GetValueOrDefault(monthKey));
                model.AttendanceMonths.Add(monthKey);
                model.AttendanceRates.Add(attendanceRates.GetValueOrDefault(monthKey));
            }

            return View(model);
        }

        #region Register (tabs page)

        /// <summary>
        /// Admin dashboard tabs page for registering Teachers, Parents, and Students separately.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ManageUsers()
        {
            ViewBag.Students = await _applicationUserService.GetUsersByTypeAsync((int)RoleEnums.Student);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterTeacher(ApplicationUser userInfo)
        {
            return await RegisterRoleAsync(userInfo, (int)RoleEnums.Teacher);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterParent(ApplicationUser userInfo)
        {
            return await RegisterRoleAsync(userInfo, (int)RoleEnums.Parent);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterStudent(ApplicationUser userInfo)
        {
            return await RegisterRoleAsync(userInfo, (int)RoleEnums.Student);
        }

        private async Task<IActionResult> RegisterRoleAsync(ApplicationUser userInfo, int userType)
        {
            userInfo.UserType = userType;
            userInfo.CreatedDate = DateTime.Now;
            userInfo.IsActive = true;

            var result = await _applicationUserService.RegisterUserAsync(userInfo);

            if (result.UserId != 0 && result.Email != null)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        #endregion

        #region List

        public async Task<IActionResult> TeacherList()
        {
            var results = await _applicationUserService.GetUsersByTypeAsync((int)RoleEnums.Teacher);
            return View(results);
        }

        public async Task<IActionResult> ParentList()
        {
            var results = await _applicationUserService.GetUsersByTypeAsync((int)RoleEnums.Parent);
            return View(results);
        }

        public async Task<IActionResult> StudentList()
        {
            var results = await _applicationUserService.GetUsersByTypeAsync((int)RoleEnums.Student);
            return View(results);
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> EditTeacher(long id)
        {
            var user = await _applicationUserService.GetUserByIdAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditTeacher(ApplicationUser userInfo)
        {
            return await UpdateRoleAsync(userInfo, nameof(TeacherList));
        }

        [HttpGet]
        public async Task<IActionResult> EditParent(long id)
        {
            var user = await _applicationUserService.GetUserByIdAsync(id);
            ViewBag.Students = await _applicationUserService.GetUsersByTypeAsync((int)RoleEnums.Student);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditParent(ApplicationUser userInfo)
        {
            return await UpdateRoleAsync(userInfo, nameof(ParentList));
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
            return await UpdateRoleAsync(userInfo, nameof(StudentList));
        }

        private async Task<IActionResult> UpdateRoleAsync(ApplicationUser userInfo, string redirectAction)
        {
            var result = await _applicationUserService.UpdateUserAsync(userInfo);
            TempData["Message"] = (result.UserId != 0) ? "Updated successfully." : "Update failed.";
            return RedirectToAction(redirectAction);
        }

        #endregion

        #region Delete (soft delete)

        [HttpPost]
        public async Task<IActionResult> DeleteTeacher(long id)
        {
            return await DeactivateRoleAsync(id, nameof(TeacherList));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteParent(long id)
        {
            return await DeactivateRoleAsync(id, nameof(ParentList));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            return await DeactivateRoleAsync(id, nameof(StudentList));
        }

        private async Task<IActionResult> DeactivateRoleAsync(long id, string redirectAction)
        {
            var result = await _applicationUserService.DeactivateUserAsync(id);
            TempData["Message"] = result ? "Deactivated successfully." : "Deactivation failed.";
            return RedirectToAction(redirectAction);
        }

        #endregion

        #region Events

        public IActionResult AddEvent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveEvent(string eventName, string description, DateTime date, string time, string place, int? grade)
        {
            var eventObject = new Events
            {
                EventName = eventName,
                Description = description,
                Date = date,
                Time = time,
                Place = place,
                Grade = grade,
                IsActive = true
            };

            try
            {
                var result = await _eventsService.SaveEventAsync(eventObject);

                if (result.Id != 0)
                {
                    return Json(new { success = true });
                }

                _logger.LogError("SaveEvent: save reported no rows affected for eventName {EventName}", eventName);
                return Json(new { success = false, message = "Save failed. No rows were written; see server logs for details." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SaveEvent: failed to save event {EventName}", eventName);
                return Json(new { success = false, message = $"Save failed: {ex.Message}" });
            }
        }

        #endregion
    }
}
