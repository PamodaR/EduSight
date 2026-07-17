using AutoMapper;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Shared.Models;

namespace ESCHOOLING.Repository.AutomapperProfiles
{
    public class EventsProfile : Profile
    {
        public EventsProfile()
        {
            CreateMap<TblEvent, Events>();
            CreateMap<Events, TblEvent>();
        }
    }
}
