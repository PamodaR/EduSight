using System.Collections.Generic;

namespace ESCHOOLING.Shared.Models
{
    public class AdminDashboardModel
    {
        public int StudentCount { get; set; }
        public int ParentCount { get; set; }
        public int TeacherCount { get; set; }
        public int CounselorCount { get; set; }
        public int AdminCount { get; set; }
        public List<string> RegistrationMonths { get; set; } = new();
        public List<int> RegistrationCounts { get; set; } = new();
        public List<string> AttendanceMonths { get; set; } = new();
        public List<double> AttendanceRates { get; set; } = new();
    }
}
