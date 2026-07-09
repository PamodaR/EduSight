using System;
using System.Collections.Generic;

namespace ESCHOOLING.DataAccess.EntityModel
{
    public partial class TblCounselor
    {
        public long CounselorId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public string? Address { get; set; }
        public string? Specialization { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
