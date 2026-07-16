using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESCHOOLING.Shared
{
    public interface IMarksService
    {
        Task<ESCHOOLING.Shared.Models.Marks> SaveMarkAsync(ESCHOOLING.Shared.Models.Marks markObject);

        /// <summary>
        /// Gets all active marks.
        /// </summary>
        Task<List<ESCHOOLING.Shared.Models.Marks>> GetAllMarksAsync();
    }
}
