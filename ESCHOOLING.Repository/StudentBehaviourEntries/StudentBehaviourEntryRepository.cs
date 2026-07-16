using AutoMapper;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.StudentBehaviourEntries
{
    public class StudentBehaviourEntryRepository : IStudentBehaviourEntryRepository
    {
        private readonly ECOM_WebContext _dbContext;
        private readonly IMapper _mapper;

        public StudentBehaviourEntryRepository(ECOM_WebContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<StudentBehaviourEntry> AddBehaviourEntryAsync(StudentBehaviourEntry entry)
        {
            var mappedObject = _mapper.Map<TblStudentBehaviourEntry>(entry);

            _dbContext.TblStudentBehaviourEntries.Add(mappedObject);
            await _dbContext.SaveChangesAsync();

            entry.Id = mappedObject.Id;
            return entry;
        }

        public async Task<List<StudentBehaviourEntry>> GetBehaviourEntriesForMonthAsync(string month)
        {
            var entries = await _dbContext.TblStudentBehaviourEntries
                .AsNoTracking()
                .Include(e => e.Student)
                .Where(e => e.IsActive == true && e.MonthForSearch == month)
                .OrderBy(e => e.CreatedDate)
                .ToListAsync();

            return entries.Select(e => new StudentBehaviourEntry
            {
                Id = e.Id,
                StudentId = e.StudentId,
                StudentName = e.Student != null ? e.Student.Username : null,
                BehaviourType = e.BehaviourType,
                Description = e.Description,
                IsActive = e.IsActive,
                CreatedDate = e.CreatedDate
            }).ToList();
        }

        public async Task<List<StudentBehaviourEntry>> GetAllBehaviourEntriesAsync()
        {
            var entries = await _dbContext.TblStudentBehaviourEntries
                .AsNoTracking()
                .Include(e => e.Student)
                .Where(e => e.IsActive == true)
                .ToListAsync();

            return entries.Select(e => new StudentBehaviourEntry
            {
                Id = e.Id,
                StudentId = e.StudentId,
                StudentName = e.Student != null ? e.Student.Username : null,
                BehaviourType = e.BehaviourType,
                Description = e.Description,
                IsActive = e.IsActive,
                CreatedDate = e.CreatedDate
            }).ToList();
        }
    }
}
