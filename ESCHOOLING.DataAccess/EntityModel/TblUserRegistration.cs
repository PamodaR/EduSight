using System;
using System.Collections.Generic;

namespace ESCHOOLING.DataAccess.EntityModel
{
    public partial class TblUserRegistration
    {
        public TblUserRegistration()
        {
            TblAttendances = new HashSet<TblAttendance>();
            TblBehaviours = new HashSet<TblBehaviour>();
            TblStudentMarks = new HashSet<TblStudentMark>();
        }

        public long UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }
        public string? MobileNo { get; set; }
        public bool? IsActive { get; set; }
        public int? UserType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Grade { get; set; }
        public bool? IsPresent { get; set; }

        public virtual ICollection<TblAttendance> TblAttendances { get; set; }
        public virtual ICollection<TblBehaviour> TblBehaviours { get; set; }
        public virtual ICollection<TblStudentMark> TblStudentMarks { get; set; }
    }
}
