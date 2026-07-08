using System;
using System.Collections.Generic;

namespace ESCHOOLING.DataAccess.EntityModel
{
    public partial class TblBehaviour
    {
        public int Id { get; set; }
        public long? StudentId { get; set; }
        public string? Behaviour1 { get; set; }
        public string? Behaviour2 { get; set; }
        public string? Behaviour3 { get; set; }
        public string? Behaviour4 { get; set; }
        public string? Behaviour5 { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual TblUserRegistration? Student { get; set; }
    }
}
