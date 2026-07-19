using ESCHOOLING.Repository.SchoolEvents;
using ESCHOOLING.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Web.Services
{
    public class EventsService : IEventsService
    {
        private readonly IEventsRepository _eventsRepository;

        public EventsService(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        public async Task<ESCHOOLING.Shared.Models.Events> SaveEventAsync(ESCHOOLING.Shared.Models.Events eventObject)
        {
            try
            {
                var result = await _eventsRepository.AddEventAsync(eventObject);
                return result;
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
                var result = await _eventsRepository.GetAllEventsAsync();
                return result;
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
                return await _eventsRepository.GetEventByIdAsync(id);
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
                return await _eventsRepository.UpdateEventAsync(eventObject);
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
                return await _eventsRepository.DeleteEventAsync(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
