using System.Collections.Generic;

namespace ESCHOOLING.Shared.Models
{
    /// <summary>
    /// View model for the Teacher "Monthly Reports" page — Attendance and Behaviour
    /// for all students, for a single selected month ("yyyy-MM").
    /// </summary>
    public class MonthlyReportsViewModel
    {
        public string SelectedMonth { get; set; } = null!;
        public List<Attendance> AttendanceEntries { get; set; } = new();
        public List<StudentBehaviourEntry> BehaviourEntries { get; set; } = new();
    }
}
