using ESCHOOLING.Repository.StudentMarks;
using ESCHOOLING.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Web.Services
{
    public class MarksService : IMarksService
    {
        private readonly IMarksRepository _marksRepository;

        public MarksService(IMarksRepository marksRepository)
        {
            _marksRepository = marksRepository;
        }

        public async Task<ESCHOOLING.Shared.Models.Marks> SaveMarkAsync(ESCHOOLING.Shared.Models.Marks markObject)
        {
            return await _marksRepository.AddMarkAsync(markObject);
        }

        /// <summary>
        /// Gets all active marks.
        /// </summary>
        public async Task<List<ESCHOOLING.Shared.Models.Marks>> GetAllMarksAsync()
        {
            try
            {
                var result = await _marksRepository.GetAllMarksAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<ESCHOOLING.Shared.Models.Marks>();
            }
        }
    }
}
