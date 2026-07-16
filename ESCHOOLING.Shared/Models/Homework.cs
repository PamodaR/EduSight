using System;

namespace ESCHOOLING.Shared.Models
{
    public class Homework
    {
        public int Id { get; set; }
        public long? TeacherId { get; set; }
        public int? Grade { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
