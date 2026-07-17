using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.SchoolEvents
{
    public interface IEventsRepository
    {
        Task<ESCHOOLING.Shared.Models.Events> AddEventAsync(ESCHOOLING.Shared.Models.Events eventObject);

        /// <summary>
        /// Gets all active events.
        /// </summary>
        Task<List<ESCHOOLING.Shared.Models.Events>> GetAllEventsAsync();
    }
}
