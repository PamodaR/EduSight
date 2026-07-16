using ESCHOOLING.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.StudentHomework
{
    public interface IHomeworkRepository
    {
        Task<ESCHOOLING.Shared.Models.Homework> AddHomeworkAsync(ESCHOOLING.Shared.Models.Homework homeworkObject);

        /// <summary>
        /// Gets all active homework.
        /// </summary>
        Task<List<ESCHOOLING.Shared.Models.Homework>> GetAllHomeworkAsync();
    }
}
