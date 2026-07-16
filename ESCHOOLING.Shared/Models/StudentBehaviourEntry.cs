using System;

namespace ESCHOOLING.Shared.Models
{
    /// <summary>
    /// A teacher-entered behaviour note for a student (one row per incident).
    /// </summary>
    public class StudentBehaviourEntry
    {
        public int Id { get; set; }
        public long StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? BehaviourType { get; set; }
        public string? Description { get; set; }
        public string? MonthForSearch { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
