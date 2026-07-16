using AutoMapper;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Shared.Models;

namespace ESCHOOLING.Repository.AutomapperProfiles
{
    public class CounsellingReferralProfile : Profile
    {
        public CounsellingReferralProfile()
        {
            CreateMap<TblCounsellingReferral, CounsellingReferral>();
            CreateMap<CounsellingReferral, TblCounsellingReferral>();
        }
    }
}
