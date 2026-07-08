using System;
using System.Collections.Generic;

namespace ESCHOOLING.DataAccess.EntityModel
{
    public partial class TblAttendance
    {
        public long Id { get; set; }
        public long StudentId { get; set; }
        public bool? IsPresent { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? Date { get; set; }
        public string? MonthForSearch { get; set; }

        public virtual TblUserRegistration Student { get; set; } = null!;
    }
}
