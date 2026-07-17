using AutoMapper;
using ESCHOOLING.DataAccess.EntityModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.SchoolEvents
{
    public class EventsRepository : IEventsRepository
    {
        private readonly ECOM_WebContext _dbContext;
        private readonly IMapper _mapper;
        private static readonly Random _random = new Random();

        public EventsRepository(ECOM_WebContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ESCHOOLING.Shared.Models.Events> AddEventAsync(ESCHOOLING.Shared.Models.Events eventObject)
        {
            try
            {
                var mappedObject = _mapper.Map<TblEvent>(eventObject);
                mappedObject.Id = _random.Next(1, int.MaxValue);

                _dbContext.TblEvents.Add(mappedObject);
                var result = await _dbContext.SaveChangesAsync();

                eventObject.Id = mappedObject.Id;
                if (result > 0) return eventObject;

                return new ESCHOOLING.Shared.Models.Events();
            }
            catch (Exception)
            {
                return new ESCHOOLING.Shared.Models.Events();
            }
        }

        /// <summary>
        /// Gets all active events.
        /// </summary>
        public async Task<List<ESCHOOLING.Shared.Models.Events>> GetAllEventsAsync()
        {
            try
            {
                var events = await _dbContext.TblEvents.AsNoTracking().Where(e => e.IsActive == true).ToListAsync();
                var mappedEvents = _mapper.Map<List<ESCHOOLING.Shared.Models.Events>>(events);
                return mappedEvents;
            }
            catch (Exception)
            {
                return new List<ESCHOOLING.Shared.Models.Events>();
            }
        }
    }
}
