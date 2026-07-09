using ESCHOOLING.Repository.StudentMarks;
using ESCHOOLING.Shared;
using System;
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
            try
            {
                var result = await _marksRepository.AddMarkAsync(markObject);
                return result;
            }
            catch (Exception)
            {
                return new ESCHOOLING.Shared.Models.Marks();
            }
        }
    }
}
