using System;
using System.Collections.Generic;

namespace ESCHOOLING.DataAccess.EntityModel
{
    public partial class TblEvent
    {
        public int Id { get; set; }
        public string? EventName { get; set; }
        public DateTime? Date { get; set; }
        public string? Place { get; set; }
        public string? Time { get; set; }
        public bool? IsActive { get; set; }
    }
}
