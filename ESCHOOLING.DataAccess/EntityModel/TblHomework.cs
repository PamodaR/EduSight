using System;
using System.Collections.Generic;

namespace ESCHOOLING.DataAccess.EntityModel
{
    public partial class TblHomework
    {
        public int Id { get; set; }
        public long? TeacherId { get; set; }
        public int? Grade { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual TblUserRegistration? Teacher { get; set; }
    }
}
