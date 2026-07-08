using ECOMSYSTEM.Shared;
using Microsoft.AspNetCore.Mvc;

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
        public ParentController(ILogger<TeacherController> logger, IApplicatioUser applicatioUserService, IConfiguration config, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _applicationUserService = applicatioUserService;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
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

        public IActionResult ViewMarks()
        {
            return View();
        }

    }
}
