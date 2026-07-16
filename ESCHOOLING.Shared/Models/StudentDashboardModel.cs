using System.Collections.Generic;

namespace ESCHOOLING.Shared.Models
{
    public class StudentDashboardModel
    {
        public double AttendanceRate { get; set; }
        public string LatestMarksAverageDisplay { get; set; } = "No marks yet";
        public List<string> AttendanceTrendMonths { get; set; } = new();
        public List<double> AttendanceTrendRates { get; set; } = new();
        public List<string> MarksSubjects { get; set; } = new();
        public List<double> MarksLatestValues { get; set; } = new();
    }
}
