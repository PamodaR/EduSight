using AutoMapper;
using ECOMSYSTEM.Shared.Models;
using ESCHOOLING.DataAccess.EntityModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Repository.Counselors
{
    public class CounselorRepository : ICounselorRepository
    {
        private readonly ECOM_WebContext _dbContext;
        private readonly IMapper _mapper;

        public CounselorRepository(ECOM_WebContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<Counselor>> GetAllAsync()
        {
            try
            {
                var counselors = await _dbContext.TblCounselors.AsNoTracking().ToListAsync();
                var mappedCounselors = _mapper.Map<List<Counselor>>(counselors);
                return mappedCounselors;
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
                var counselor = await _dbContext.TblCounselors.AsNoTracking().Where(data => data.CounselorId.Equals(id)).FirstOrDefaultAsync();
                var mappedCounselor = _mapper.Map<Counselor>(counselor);
                return mappedCounselor;
            }
            catch (Exception)
            {
                return new Counselor();
            }
        }

        public async Task<Counselor> AddCounselorAsync(Counselor counselorObject)
        {
            try
            {
                var mappedObject = _mapper.Map<TblCounselor>(counselorObject);

                _dbContext.TblCounselors.Add(mappedObject);
                var result = await _dbContext.SaveChangesAsync();

                counselorObject.CounselorId = mappedObject.CounselorId;
                if (result > 0) return counselorObject;

                return new Counselor();
            }
            catch (Exception)
            {
                return new Counselor();
            }
        }

        public async Task<Counselor> UpdateCounselorAsync(Counselor counselorObject)
        {
            try
            {
                var result = await _dbContext.TblCounselors.Where(data => data.CounselorId.Equals(counselorObject.CounselorId)).FirstOrDefaultAsync();
                if (result == null)
                {
                    return new Counselor();
                }

                result.Name = counselorObject.Name;
                result.Email = counselorObject.Email;
                result.MobileNo = counselorObject.MobileNo;
                result.Address = counselorObject.Address;
                result.Specialization = counselorObject.Specialization;
                result.IsActive = counselorObject.IsActive;

                await _dbContext.SaveChangesAsync();

                var mappedObject = _mapper.Map<Counselor>(result);
                return mappedObject;
            }
            catch (Exception)
            {
                return new Counselor();
            }
        }

        public async Task<bool> DeleteCounselorAsync(long id)
        {
            try
            {
                var result = await _dbContext.TblCounselors.Where(data => data.CounselorId.Equals(id)).FirstOrDefaultAsync();
                if (result == null)
                {
                    return false;
                }

                _dbContext.TblCounselors.Remove(result);
                var saved = await _dbContext.SaveChangesAsync();
                return saved > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
