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

        public async Task<ESCHOOLING.Shared.Models.Events?> GetEventByIdAsync(int id)
        {
            try
            {
                var eventEntity = await _dbContext.TblEvents.AsNoTracking().Where(e => e.Id == id).FirstOrDefaultAsync();
                return eventEntity == null ? null : _mapper.Map<ESCHOOLING.Shared.Models.Events>(eventEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ESCHOOLING.Shared.Models.Events> UpdateEventAsync(ESCHOOLING.Shared.Models.Events eventObject)
        {
            try
            {
                var result = await _dbContext.TblEvents.Where(e => e.Id == eventObject.Id).FirstOrDefaultAsync();
                if (result == null)
                {
                    return new ESCHOOLING.Shared.Models.Events();
                }

                result.EventName = eventObject.EventName;
                result.Description = eventObject.Description;
                result.Date = eventObject.Date;
                result.Time = eventObject.Time;
                result.Place = eventObject.Place;
                result.Grade = eventObject.Grade;

                await _dbContext.SaveChangesAsync();

                return _mapper.Map<ESCHOOLING.Shared.Models.Events>(result);
            }
            catch (Exception)
            {
                return new ESCHOOLING.Shared.Models.Events();
            }
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            try
            {
                var result = await _dbContext.TblEvents.Where(e => e.Id == id).FirstOrDefaultAsync();
                if (result == null)
                {
                    return false;
                }

                _dbContext.TblEvents.Remove(result);
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
