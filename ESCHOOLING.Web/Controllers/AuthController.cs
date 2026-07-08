using ECOMSYSTEM.Shared;
using ECOMSYSTEM.Shared.Enum;
using ECOMSYSTEM.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace ECOMSYSTEM.Web.Controllers
{
    public class AuthController : Controller
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _config;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<AuthController> _logger;
        /// <summary>
        /// The application user service
        /// </summary>
        private readonly IApplicatioUser _applicationUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="applicatioUserService">The applicatio user service.</param>
        /// <param name="config">The configuration.</param>
        public AuthController(ILogger<AuthController> logger, IApplicatioUser applicatioUserService, IConfiguration config)
        {
            _logger = logger;
            _applicationUserService = applicatioUserService;
            _config = config;
        }

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Registers the specified application user.
        /// </summary>
        /// <param name="applicationUser">The application user.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Register(ApplicationUser applicationUser)
        {
             applicationUser.CreatedDate = DateTime.Now;

            var user = _applicationUserService.RegisterUser(applicationUser);

            if (user.UserId != 0 && user.Email != null)
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
        /// Logins this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult login()
        {
            return View();
        }

        /// <summary>Logins the specified user.</summary>
        /// <param name="linfo">The linfo.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost]
        public IActionResult Login(ApplicationUser linfo)
        {
            var result = _applicationUserService.LoginUser(linfo);
            if (result.Email == null)
            {
                return Json(new
                {
                    success = false
                });
            }

            ApplicationSession.applicationUserId = result.UserId;

            var role = ((RoleEnums)result.UserType).ToString();

            if (role == "Admin")
            {
                return Json(new
                {
                    success = true,
                    newUrl = Url.Action("AdminHome", "Admin")
                });
            }
            else if(role == "Teacher") 
            {
                return Json(new
                {
                    success = true,
                    newUrl = Url.Action("TeacherHome", "Teacher")
                });
            }
            else if(role == "Student")
            {
                return Json(new
                {
                    success = true,
                    newUrl = Url.Action("StudentHome", "Student")
                });
            }
            else
            {
                return Json(new
                {
                    success = true,
                    newUrl = Url.Action("ParentHome", "Parent")
                });
            }

        }
    }
}
