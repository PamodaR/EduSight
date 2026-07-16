using System.Collections.Generic;

namespace ESCHOOLING.Shared.Models
{
    public class TeacherDashboardModel
    {
        public int TotalStudentsManaged { get; set; }
        public double TodayAttendanceRate { get; set; }
        public int TodayPresentCount { get; set; }
        public int TodayTotalMarkedCount { get; set; }
        public List<string> AttendanceTrendMonths { get; set; } = new();
        public List<double> AttendanceTrendRates { get; set; } = new();
        public List<string> MarksDistributionBuckets { get; set; } = new();
        public List<int> MarksDistributionCounts { get; set; } = new();
    }
}
