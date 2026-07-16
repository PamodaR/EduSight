using ESCHOOLING.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.CounsellingReferrals
{
    public interface ICounsellingReferralRepository
    {
        Task<CounsellingReferral> AddReferralAsync(CounsellingReferral referral);

        /// <summary>
        /// Gets all active referrals, with student, counselor, and teacher names populated for display.
        /// </summary>
        Task<List<CounsellingReferral>> GetAllReferralsAsync();
    }
}
