using ESCHOOLING.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESCHOOLING.Shared
{
    public interface IParentNoteService
    {
        Task<ParentNote> SaveNoteAsync(ParentNote note);

        /// <summary>
        /// Gets all active parent notes, with parent and student names populated for display.
        /// </summary>
        Task<List<ParentNote>> GetAllNotesAsync();
    }
}
