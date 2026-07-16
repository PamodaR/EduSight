using AutoMapper;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Shared.Models;

namespace ESCHOOLING.Repository.AutomapperProfiles
{
    public class ParentNoteProfile : Profile
    {
        public ParentNoteProfile()
        {
            CreateMap<TblParentNote, ParentNote>();
            CreateMap<ParentNote, TblParentNote>();
        }
    }
}
