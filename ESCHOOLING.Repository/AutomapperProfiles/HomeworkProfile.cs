using AutoMapper;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Shared.Models;

namespace ESCHOOLING.Repository.AutomapperProfiles
{
    public class HomeworkProfile : Profile
    {
        public HomeworkProfile()
        {
            CreateMap<TblHomework, Homework>();
            CreateMap<Homework, TblHomework>();
        }
    }
}
