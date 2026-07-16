using ESCHOOLING.Repository.StudentBehaviourEntries;
using ESCHOOLING.Shared;
using ESCHOOLING.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Web.Services
{
    public class StudentBehaviourEntryService : IStudentBehaviourEntryService
    {
        private readonly IStudentBehaviourEntryRepository _studentBehaviourEntryRepository;

        public StudentBehaviourEntryService(IStudentBehaviourEntryRepository studentBehaviourEntryRepository)
        {
            _studentBehaviourEntryRepository = studentBehaviourEntryRepository;
        }

        public async Task<StudentBehaviourEntry> SaveBehaviourEntryAsync(StudentBehaviourEntry entry)
        {
            return await _studentBehaviourEntryRepository.AddBehaviourEntryAsync(entry);
        }

        public async Task<List<StudentBehaviourEntry>> GetAllBehaviourEntriesAsync()
        {
            return await _studentBehaviourEntryRepository.GetAllBehaviourEntriesAsync();
        }

        public async Task<List<StudentBehaviourEntry>> GetBehaviourEntriesForMonthAsync(string month)
        {
            return await _studentBehaviourEntryRepository.GetBehaviourEntriesForMonthAsync(month);
        }
    }
}
