using ESCHOOLING.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.ParentNotes
{
    public interface IParentNoteRepository
    {
        Task<ParentNote> AddNoteAsync(ParentNote note);

        /// <summary>
        /// Gets all active parent notes, with parent and student names populated for display.
        /// </summary>
        Task<List<ParentNote>> GetAllNotesAsync();
    }
}
