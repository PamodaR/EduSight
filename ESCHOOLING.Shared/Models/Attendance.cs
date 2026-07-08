using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESCHOOLING.Shared.Models
{
    public class Attendance
    {
        public long Id { get; set; }
        public long StudentId { get; set; }
        public bool? IsPresent { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? Date { get; set; }
        public string? MonthForSearch { get; set; }
    }
}
