using ESCHOOLING.Repository.ParentNotes;
using ESCHOOLING.Shared;
using ESCHOOLING.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Web.Services
{
    public class ParentNoteService : IParentNoteService
    {
        private readonly IParentNoteRepository _parentNoteRepository;

        public ParentNoteService(IParentNoteRepository parentNoteRepository)
        {
            _parentNoteRepository = parentNoteRepository;
        }

        public async Task<ParentNote> SaveNoteAsync(ParentNote note)
        {
            return await _parentNoteRepository.AddNoteAsync(note);
        }

        public async Task<List<ParentNote>> GetAllNotesAsync()
        {
            return await _parentNoteRepository.GetAllNotesAsync();
        }
    }
}
