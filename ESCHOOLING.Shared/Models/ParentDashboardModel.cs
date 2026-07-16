using System.Collections.Generic;

namespace ESCHOOLING.Shared.Models
{
    public class ParentDashboardModel
    {
        public double AttendanceRate { get; set; }
        public string LatestMarksAverageDisplay { get; set; } = "No marks yet";
        public int BehaviourPositiveCount { get; set; }
        public int BehaviourNegativeCount { get; set; }
        public int BehaviourNeutralCount { get; set; }
        public List<string> AttendanceTrendMonths { get; set; } = new();
        public List<double> AttendanceTrendRates { get; set; } = new();
        public List<string> MarksSubjects { get; set; } = new();
        public List<double> MarksLatestValues { get; set; } = new();
    }
}
