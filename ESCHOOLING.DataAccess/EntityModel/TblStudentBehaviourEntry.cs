using System;
using System.Collections.Generic;

namespace ESCHOOLING.DataAccess.EntityModel
{
    public partial class TblStudentBehaviourEntry
    {
        public int Id { get; set; }
        public long StudentId { get; set; }
        public string BehaviourType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? MonthForSearch { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual TblUserRegistration? Student { get; set; }
    }
}
