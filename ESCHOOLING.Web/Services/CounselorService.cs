using ECOMSYSTEM.Repository.Counselors;
using ECOMSYSTEM.Shared;
using ECOMSYSTEM.Shared.Models;

namespace ECOMSYSTEM.Web.Services
{
    public class CounselorService : ICounselorService
    {
        private readonly ICounselorRepository _counselorRepository;

        public CounselorService(ICounselorRepository counselorRepository)
        {
            _counselorRepository = counselorRepository;
        }

        public async Task<List<Counselor>> GetAllAsync()
        {
            try
            {
                var result = await _counselorRepository.GetAllAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<Counselor>();
            }
        }

        public async Task<Counselor> GetByIdAsync(long id)
        {
            try
            {
                var result = await _counselorRepository.GetByIdAsync(id);
                return result;
            }
            catch (Exception)
            {
                return new Counselor();
            }
        }

        public async Task<Counselor> CreateAsync(Counselor counselorObject)
        {
            try
            {
                var result = await _counselorRepository.AddCounselorAsync(counselorObject);
                return result;
            }
            catch (Exception)
            {
                return new Counselor();
            }
        }

        public async Task<Counselor> UpdateAsync(Counselor counselorObject)
        {
            try
            {
                var result = await _counselorRepository.UpdateCounselorAsync(counselorObject);
                return result;
            }
            catch (Exception)
            {
                return new Counselor();
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var result = await _counselorRepository.DeleteCounselorAsync(id);
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
