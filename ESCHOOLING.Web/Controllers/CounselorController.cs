using ECOMSYSTEM.Shared;
using ECOMSYSTEM.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCHOOLING.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CounselorController : Controller
    {
        /// <summary>
        /// The counselor service
        /// </summary>
        private readonly ICounselorService _counselorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CounselorController"/> class.
        /// </summary>
        /// <param name="counselorService">The counselor service.</param>
        public CounselorController(ICounselorService counselorService)
        {
            _counselorService = counselorService;
        }

        public async Task<IActionResult> Index()
        {
            var results = await _counselorService.GetAllAsync();
            return View(results);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Counselor counselorInfo)
        {
            counselorInfo.CreatedDate = DateTime.Now;
            counselorInfo.IsActive = true;

            var result = await _counselorService.CreateAsync(counselorInfo);

            if (result.CounselorId != 0)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var counselor = await _counselorService.GetByIdAsync(id);
            return View(counselor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Counselor counselorInfo)
        {
            var result = await _counselorService.UpdateAsync(counselorInfo);
            TempData["Message"] = (result.CounselorId != 0) ? "Updated successfully." : "Update failed.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _counselorService.DeleteAsync(id);
            TempData["Message"] = result ? "Deleted successfully." : "Delete failed.";

            return RedirectToAction(nameof(Index));
        }
    }
}
