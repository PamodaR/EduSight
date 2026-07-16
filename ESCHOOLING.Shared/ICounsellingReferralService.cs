using ESCHOOLING.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESCHOOLING.Shared
{
    public interface ICounsellingReferralService
    {
        Task<CounsellingReferral> SaveReferralAsync(CounsellingReferral referral);

        /// <summary>
        /// Gets all active referrals, with student, counselor, and teacher names populated for display.
        /// </summary>
        Task<List<CounsellingReferral>> GetAllReferralsAsync();
    }
}
