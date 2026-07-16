using ESCHOOLING.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESCHOOLING.Shared
{
    public interface IStudentMarksEntryService
    {
        Task<StudentMarksEntry> SaveMarksEntryAsync(StudentMarksEntry entry);

        /// <summary>
        /// Gets all active marks entries, with the student's name populated for display.
        /// </summary>
        Task<List<StudentMarksEntry>> GetAllMarksEntriesAsync();
    }
}
