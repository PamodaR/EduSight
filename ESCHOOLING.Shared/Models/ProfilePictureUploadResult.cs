namespace ECOMSYSTEM.Shared.Models
{
    public class ProfilePictureUploadResult
    {
        public bool Success { get; set; }
        public string? ImageUrl { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
