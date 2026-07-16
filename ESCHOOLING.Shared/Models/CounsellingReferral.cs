using System;

namespace ESCHOOLING.Shared.Models
{
    /// <summary>
    /// A teacher's referral of a student to a counselor. Triggers a best-effort
    /// notification email to the counselor's address when saved.
    /// </summary>
    public class CounsellingReferral
    {
        public int Id { get; set; }
        public long StudentId { get; set; }
        public string? StudentName { get; set; }
        public long CounselorId { get; set; }
        public string? CounselorName { get; set; }
        public string? CounselorEmail { get; set; }
        public long TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string? Reason { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
