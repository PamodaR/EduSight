using AutoMapper;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Shared.Models;

namespace ESCHOOLING.Repository.AutomapperProfiles
{
    public class MarksProfile : Profile
    {
        public MarksProfile()
        {
            CreateMap<TblStudentMark, Marks>();
            CreateMap<Marks, TblStudentMark>();
        }
    }
}
