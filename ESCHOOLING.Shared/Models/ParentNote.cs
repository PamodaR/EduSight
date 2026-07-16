using System;

namespace ESCHOOLING.Shared.Models
{
    /// <summary>
    /// A free-text note a parent sends to their linked child's teacher(s).
    /// </summary>
    public class ParentNote
    {
        public int Id { get; set; }
        public long ParentId { get; set; }
        public string? ParentName { get; set; }
        public long StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? NoteText { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
