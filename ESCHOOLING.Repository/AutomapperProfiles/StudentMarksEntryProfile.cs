using AutoMapper;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Shared.Models;

namespace ESCHOOLING.Repository.AutomapperProfiles
{
    public class StudentMarksEntryProfile : Profile
    {
        public StudentMarksEntryProfile()
        {
            CreateMap<TblStudentMarksEntry, StudentMarksEntry>();
            CreateMap<StudentMarksEntry, TblStudentMarksEntry>();
        }
    }
}
