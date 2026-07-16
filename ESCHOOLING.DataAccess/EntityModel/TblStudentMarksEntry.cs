using System;
using System.Collections.Generic;

namespace ESCHOOLING.DataAccess.EntityModel
{
    public partial class TblStudentMarksEntry
    {
        public int Id { get; set; }
        public long StudentId { get; set; }
        public string Term { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public decimal Marks { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual TblUserRegistration? Student { get; set; }
    }
}
