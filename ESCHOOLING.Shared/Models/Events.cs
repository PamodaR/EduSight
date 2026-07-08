using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESCHOOLING.Shared.Models
{
    public class Events
    {
        public int Id { get; set; }
        public string? EventName { get; set; }
        public DateTime? Date { get; set; }
        public string? Place { get; set; }
        public string? Time { get; set; }
        public bool? IsActive { get; set; }
    }
}
