using AutoMapper;
using ESCHOOLING.DataAccess.EntityModel;
using System;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.StudentMarks
{
    public class MarksRepository : IMarksRepository
    {
        private readonly ECOM_WebContext _dbContext;
        private readonly IMapper _mapper;
        private static readonly Random _random = new Random();

        public MarksRepository(ECOM_WebContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ESCHOOLING.Shared.Models.Marks> AddMarkAsync(ESCHOOLING.Shared.Models.Marks markObject)
        {
            try
            {
                var mappedObject = _mapper.Map<TblStudentMark>(markObject);
                mappedObject.Id = _random.Next(1, int.MaxValue);

                _dbContext.TblStudentMarks.Add(mappedObject);
                var result = await _dbContext.SaveChangesAsync();

                markObject.Id = mappedObject.Id;
                if (result > 0) return markObject;

                return new ESCHOOLING.Shared.Models.Marks();
            }
            catch (Exception)
            {
                return new ESCHOOLING.Shared.Models.Marks();
            }
        }
    }
}
