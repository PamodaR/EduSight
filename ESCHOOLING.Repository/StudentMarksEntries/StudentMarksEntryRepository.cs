using AutoMapper;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.StudentMarksEntries
{
    public class StudentMarksEntryRepository : IStudentMarksEntryRepository
    {
        private readonly ECOM_WebContext _dbContext;
        private readonly IMapper _mapper;

        public StudentMarksEntryRepository(ECOM_WebContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<StudentMarksEntry> AddMarksEntryAsync(StudentMarksEntry entry)
        {
            var mappedObject = _mapper.Map<TblStudentMarksEntry>(entry);

            _dbContext.TblStudentMarksEntries.Add(mappedObject);
            await _dbContext.SaveChangesAsync();

            entry.Id = mappedObject.Id;
            return entry;
        }

        public async Task<List<StudentMarksEntry>> GetAllMarksEntriesAsync()
        {
            var entries = await _dbContext.TblStudentMarksEntries
                .AsNoTracking()
                .Include(e => e.Student)
                .Where(e => e.IsActive == true)
                .ToListAsync();

            return entries.Select(e => new StudentMarksEntry
            {
                Id = e.Id,
                StudentId = e.StudentId,
                StudentName = e.Student != null ? e.Student.Username : null,
                Term = e.Term,
                Subject = e.Subject,
                Marks = e.Marks,
                IsActive = e.IsActive,
                CreatedDate = e.CreatedDate
            }).ToList();
        }
    }
}
