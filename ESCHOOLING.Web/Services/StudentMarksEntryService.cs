using ESCHOOLING.Repository.StudentMarksEntries;
using ESCHOOLING.Shared;
using ESCHOOLING.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Web.Services
{
    public class StudentMarksEntryService : IStudentMarksEntryService
    {
        private readonly IStudentMarksEntryRepository _studentMarksEntryRepository;

        public StudentMarksEntryService(IStudentMarksEntryRepository studentMarksEntryRepository)
        {
            _studentMarksEntryRepository = studentMarksEntryRepository;
        }

        public async Task<StudentMarksEntry> SaveMarksEntryAsync(StudentMarksEntry entry)
        {
            return await _studentMarksEntryRepository.AddMarksEntryAsync(entry);
        }

        public async Task<List<StudentMarksEntry>> GetAllMarksEntriesAsync()
        {
            return await _studentMarksEntryRepository.GetAllMarksEntriesAsync();
        }
    }
}
