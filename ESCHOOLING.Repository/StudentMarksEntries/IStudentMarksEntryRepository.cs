using ESCHOOLING.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.StudentMarksEntries
{
    public interface IStudentMarksEntryRepository
    {
        Task<StudentMarksEntry> AddMarksEntryAsync(StudentMarksEntry entry);

        /// <summary>
        /// Gets all active marks entries, with the student's name populated for display.
        /// </summary>
        Task<List<StudentMarksEntry>> GetAllMarksEntriesAsync();
    }
}
