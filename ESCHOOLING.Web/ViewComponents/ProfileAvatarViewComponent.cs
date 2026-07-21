using ECOMSYSTEM.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ESCHOOLING.Web.ViewComponents
{
    public class ProfileAvatarViewComponent : ViewComponent
    {
        private const string FallbackImagePath = "/dist-assets/images/User.png";
        private readonly IApplicatioUser _applicationUserService;

        public ProfileAvatarViewComponent(IApplicatioUser applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userIdClaim = UserClaimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return View("Default", FallbackImagePath);
            }

            var user = await _applicationUserService.GetUserByIdAsync(userId);
            var imagePath = string.IsNullOrEmpty(user?.ProfilePicturePath) ? FallbackImagePath : user.ProfilePicturePath;
            return View("Default", imagePath);
        }
    }
}
