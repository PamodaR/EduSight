using AutoMapper;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.CounsellingReferrals
{
    public class CounsellingReferralRepository : ICounsellingReferralRepository
    {
        private readonly ECOM_WebContext _dbContext;
        private readonly IMapper _mapper;

        public CounsellingReferralRepository(ECOM_WebContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CounsellingReferral> AddReferralAsync(CounsellingReferral referral)
        {
            var mappedObject = _mapper.Map<TblCounsellingReferral>(referral);

            _dbContext.TblCounsellingReferrals.Add(mappedObject);
            await _dbContext.SaveChangesAsync();

            referral.Id = mappedObject.Id;
            return referral;
        }

        public async Task<List<CounsellingReferral>> GetAllReferralsAsync()
        {
            var referrals = await _dbContext.TblCounsellingReferrals
                .AsNoTracking()
                .Include(r => r.Student)
                .Include(r => r.Counselor)
                .Include(r => r.Teacher)
                .Where(r => r.IsActive == true)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            return referrals.Select(r => new CounsellingReferral
            {
                Id = r.Id,
                StudentId = r.StudentId,
                StudentName = r.Student != null ? r.Student.Username : null,
                CounselorId = r.CounselorId,
                CounselorName = r.Counselor != null ? r.Counselor.Name : null,
                CounselorEmail = r.Counselor != null ? r.Counselor.Email : null,
                TeacherId = r.TeacherId,
                TeacherName = r.Teacher != null ? r.Teacher.Username : null,
                Reason = r.Reason,
                IsActive = r.IsActive,
                CreatedDate = r.CreatedDate
            }).ToList();
        }
    }
}
