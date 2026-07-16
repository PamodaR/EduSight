using System;

namespace ESCHOOLING.DataAccess.EntityModel
{
    public partial class TblParentNote
    {
        public int Id { get; set; }
        public long ParentId { get; set; }
        public long StudentId { get; set; }
        public string NoteText { get; set; } = null!;
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual TblUserRegistration? Parent { get; set; }
        public virtual TblUserRegistration? Student { get; set; }
    }
}
