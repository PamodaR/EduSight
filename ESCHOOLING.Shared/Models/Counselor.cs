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
        /// <summary>
        /// Non-nullable view of <see cref="IsActive"/> for checkbox binding — asp-for's CheckBoxTagHelper
        /// only supports bool/string, not bool?.
        /// </summary>
        public bool IsActiveChecked
        {
            get => IsActive ?? false;
            set => IsActive = value;
        }
        public DateTime? CreatedDate { get; set; }
    }
}
