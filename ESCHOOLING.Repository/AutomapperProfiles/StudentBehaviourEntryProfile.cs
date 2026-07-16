using AutoMapper;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Shared.Models;

namespace ESCHOOLING.Repository.AutomapperProfiles
{
    public class StudentBehaviourEntryProfile : Profile
    {
        public StudentBehaviourEntryProfile()
        {
            CreateMap<TblStudentBehaviourEntry, StudentBehaviourEntry>();
            CreateMap<StudentBehaviourEntry, TblStudentBehaviourEntry>();
        }
    }
}
