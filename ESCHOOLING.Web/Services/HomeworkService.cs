using ESCHOOLING.Repository.StudentHomework;
using ESCHOOLING.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Web.Services
{
    public class HomeworkService : IHomeworkService
    {
        private readonly IHomeworkRepository _homeworkRepository;

        public HomeworkService(IHomeworkRepository homeworkRepository)
        {
            _homeworkRepository = homeworkRepository;
        }

        public async Task<ESCHOOLING.Shared.Models.Homework> SaveHomeworkAsync(ESCHOOLING.Shared.Models.Homework homeworkObject)
        {
            try
            {
                var result = await _homeworkRepository.AddHomeworkAsync(homeworkObject);
                return result;
            }
            catch (Exception)
            {
                return new ESCHOOLING.Shared.Models.Homework();
            }
        }

        /// <summary>
        /// Gets all active homework.
        /// </summary>
        public async Task<List<ESCHOOLING.Shared.Models.Homework>> GetAllHomeworkAsync()
        {
            try
            {
                var result = await _homeworkRepository.GetAllHomeworkAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<ESCHOOLING.Shared.Models.Homework>();
            }
        }
    }
}
