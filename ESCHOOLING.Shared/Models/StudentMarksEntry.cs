using System;

namespace ESCHOOLING.Shared.Models
{
    /// <summary>
    /// A real, teacher-entered exam/test mark. Independent of <see cref="Marks"/>
    /// (the ONNX Predict Mark system) — the two are never mixed.
    /// </summary>
    public class StudentMarksEntry
    {
        public int Id { get; set; }
        public long StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? Term { get; set; }
        public string? Subject { get; set; }
        public decimal Marks { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
