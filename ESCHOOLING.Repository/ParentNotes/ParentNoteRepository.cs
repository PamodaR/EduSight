using AutoMapper;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.ParentNotes
{
    public class ParentNoteRepository : IParentNoteRepository
    {
        private readonly ECOM_WebContext _dbContext;
        private readonly IMapper _mapper;

        public ParentNoteRepository(ECOM_WebContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ParentNote> AddNoteAsync(ParentNote note)
        {
            var mappedObject = _mapper.Map<TblParentNote>(note);

            _dbContext.TblParentNotes.Add(mappedObject);
            await _dbContext.SaveChangesAsync();

            note.Id = mappedObject.Id;
            return note;
        }

        public async Task<List<ParentNote>> GetAllNotesAsync()
        {
            var notes = await _dbContext.TblParentNotes
                .AsNoTracking()
                .Include(n => n.Parent)
                .Include(n => n.Student)
                .Where(n => n.IsActive == true)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();

            return notes.Select(n => new ParentNote
            {
                Id = n.Id,
                ParentId = n.ParentId,
                ParentName = n.Parent != null ? n.Parent.Username : null,
                StudentId = n.StudentId,
                StudentName = n.Student != null ? n.Student.Username : null,
                NoteText = n.NoteText,
                IsActive = n.IsActive,
                CreatedDate = n.CreatedDate
            }).ToList();
        }
    }
}
