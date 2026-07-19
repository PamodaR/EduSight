using ECOMSYSTEM.Shared;
using ECOMSYSTEM.Shared.Enum;
using ECOMSYSTEM.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        public async Task<IActionResult> Login(ApplicationUser linfo)
        {
            var result = await _applicationUserService.LoginUserAsync(linfo);
            if (result.Email == null)
            {
                return Json(new
                {
                    success = false
                });
            }

            var role = ((RoleEnums)result.UserType).ToString();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
                new Claim(ClaimTypes.Name, result.Username ?? string.Empty),
                new Claim(ClaimTypes.Email, result.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

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

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login", "Auth");
        }
    }
}
