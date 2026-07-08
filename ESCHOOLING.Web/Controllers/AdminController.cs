using ECOMSYSTEM.Shared;
using ECOMSYSTEM.Shared.Models;
using Microsoft.AspNetCore.Mvc;

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
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="applicatioUserService">The applicatio user service.</param>
        /// <param name="config">The configuration.</param>
        public AdminController(ILogger<AdminController> logger, IApplicatioUser applicatioUserService, IConfiguration config, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _applicationUserService = applicatioUserService;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult AdminHome()
        {
            TeacherDashboadModel adminDashboadModel = new TeacherDashboadModel();
            adminDashboadModel.StudentCount = 0;
            adminDashboadModel.ParentCount = 0;
            adminDashboadModel.AdmintCount = 0;
            adminDashboadModel.TeacherCount = 0;

            var users = _applicationUserService.GetAllUsers();
            if (users.Count > 0)
            {
                foreach (var user in users)
                {
                    if (user.UserType == 0)
                    {
                        adminDashboadModel.AdmintCount++;
                    }
                    else if (user.UserType == 1)
                    {
                        adminDashboadModel.TeacherCount++;
                    }
                    else if (user.UserType == 2)
                    {
                        adminDashboadModel.StudentCount++;
                    }
                    else
                    {
                        adminDashboadModel.ParentCount++;
                    }
                }
            }

            return View(adminDashboadModel);
        }
    }
}
