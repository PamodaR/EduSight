using ESCHOOLING.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.StudentBehaviourEntries
{
    public interface IStudentBehaviourEntryRepository
    {
        Task<StudentBehaviourEntry> AddBehaviourEntryAsync(StudentBehaviourEntry entry);

        /// <summary>
        /// Gets all active behaviour entries, with the student's name populated for display.
        /// </summary>
        Task<List<StudentBehaviourEntry>> GetAllBehaviourEntriesAsync();

        /// <summary>
        /// Gets all active behaviour entries for a given month (format "yyyy-MM"), across all students.
        /// </summary>
        Task<List<StudentBehaviourEntry>> GetBehaviourEntriesForMonthAsync(string month);
    }
}
