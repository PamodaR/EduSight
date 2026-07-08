using System;
using System.Collections.Generic;

namespace ESCHOOLING.DataAccess.EntityModel
{
    public partial class TblStudentMark
    {
        public int Id { get; set; }
        public long? StudentId { get; set; }
        public string? Subject { get; set; }
        public string? PredictedMark { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual TblUserRegistration? Student { get; set; }
    }
}
