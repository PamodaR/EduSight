using ESCHOOLING.Repository.CounsellingReferrals;
using ESCHOOLING.Shared;
using ESCHOOLING.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Web.Services
{
    public class CounsellingReferralService : ICounsellingReferralService
    {
        private readonly ICounsellingReferralRepository _counsellingReferralRepository;

        public CounsellingReferralService(ICounsellingReferralRepository counsellingReferralRepository)
        {
            _counsellingReferralRepository = counsellingReferralRepository;
        }

        public async Task<CounsellingReferral> SaveReferralAsync(CounsellingReferral referral)
        {
            return await _counsellingReferralRepository.AddReferralAsync(referral);
        }

        public async Task<List<CounsellingReferral>> GetAllReferralsAsync()
        {
            return await _counsellingReferralRepository.GetAllReferralsAsync();
        }
    }
}
