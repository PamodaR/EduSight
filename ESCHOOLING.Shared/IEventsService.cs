using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESCHOOLING.Shared
{
    public interface IEventsService
    {
        Task<ESCHOOLING.Shared.Models.Events> SaveEventAsync(ESCHOOLING.Shared.Models.Events eventObject);

        /// <summary>
        /// Gets all active events.
        /// </summary>
        Task<List<ESCHOOLING.Shared.Models.Events>> GetAllEventsAsync();

        Task<ESCHOOLING.Shared.Models.Events?> GetEventByIdAsync(int id);

        Task<ESCHOOLING.Shared.Models.Events> UpdateEventAsync(ESCHOOLING.Shared.Models.Events eventObject);

        Task<bool> DeleteEventAsync(int id);
    }
}
