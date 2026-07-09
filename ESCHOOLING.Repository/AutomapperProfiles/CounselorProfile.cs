using AutoMapper;
using ECOMSYSTEM.Shared.Models;
using ESCHOOLING.DataAccess.EntityModel;

namespace ECOMSYSTEM.Repository.AutomapperProfiles
{
    public class CounselorProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CounselorProfile"/> class.
        /// </summary>
        public CounselorProfile()
        {
            CreateMap<TblCounselor, Counselor>();
            CreateMap<Counselor, TblCounselor>();
        }
    }
}
