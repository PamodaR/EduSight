using ECOMSYSTEM.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Shared
{
    public interface ICounselorService
    {
        Task<List<Counselor>> GetAllAsync();
        Task<Counselor> GetByIdAsync(long id);
        Task<Counselor> CreateAsync(Counselor counselorObject);
        Task<Counselor> UpdateAsync(Counselor counselorObject);
        Task<bool> DeleteAsync(long id);

        /// <summary>
        /// Gets the count of active counselors.
        /// </summary>
        Task<int> GetActiveCounselorCountAsync();
    }
}
