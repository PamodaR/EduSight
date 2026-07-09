using System;
using System.ComponentModel.DataAnnotations;

namespace ECOMSYSTEM.Shared.Models
{
    public class Counselor
    {
        public long CounselorId { get; set; }
        public string? Name { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? MobileNo { get; set; }
        public string? Address { get; set; }
        public string? Specialization { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
