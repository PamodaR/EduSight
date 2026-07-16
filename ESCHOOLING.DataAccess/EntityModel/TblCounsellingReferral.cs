using System;

namespace ESCHOOLING.DataAccess.EntityModel
{
    public partial class TblCounsellingReferral
    {
        public int Id { get; set; }
        public long StudentId { get; set; }
        public long CounselorId { get; set; }
        public long TeacherId { get; set; }
        public string Reason { get; set; } = null!;
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual TblUserRegistration? Student { get; set; }
        public virtual TblCounselor? Counselor { get; set; }
        public virtual TblUserRegistration? Teacher { get; set; }
    }
}
