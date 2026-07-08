using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESCHOOLING.Shared.Models
{
    public class Marks
    {
        public int Id { get; set; }
        public long? StudentId { get; set; }
        public string? Subject { get; set; }
        public string? PredictedMark { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
