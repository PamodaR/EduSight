using ECOMSYSTEM.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Repository.Counselors
{
    public interface ICounselorRepository
    {
        Task<List<Counselor>> GetAllAsync();
        Task<Counselor> GetByIdAsync(long id);
        Task<Counselor> AddCounselorAsync(Counselor counselorObject);
        Task<Counselor> UpdateCounselorAsync(Counselor counselorObject);
        Task<bool> DeleteCounselorAsync(long id);
    }
}
