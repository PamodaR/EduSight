using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESCHOOLING.Shared
{
    public interface IHomeworkService
    {
        Task<ESCHOOLING.Shared.Models.Homework> SaveHomeworkAsync(ESCHOOLING.Shared.Models.Homework homeworkObject);

        /// <summary>
        /// Gets all active homework.
        /// </summary>
        Task<List<ESCHOOLING.Shared.Models.Homework>> GetAllHomeworkAsync();
    }
}
