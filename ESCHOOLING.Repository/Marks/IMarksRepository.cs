using ESCHOOLING.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.StudentMarks
{
    public interface IMarksRepository
    {
        Task<ESCHOOLING.Shared.Models.Marks> AddMarkAsync(ESCHOOLING.Shared.Models.Marks markObject);

        /// <summary>
        /// Gets all active marks.
        /// </summary>
        Task<List<ESCHOOLING.Shared.Models.Marks>> GetAllMarksAsync();
    }
}
