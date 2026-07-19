using AutoMapper;
using ESCHOOLING.DataAccess.EntityModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.StudentMarks
{
    public class MarksRepository : IMarksRepository
    {
        private readonly ECOM_WebContext _dbContext;
        private readonly IMapper _mapper;

        public MarksRepository(ECOM_WebContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ESCHOOLING.Shared.Models.Marks> AddMarkAsync(ESCHOOLING.Shared.Models.Marks markObject)
        {
            var mappedObject = _mapper.Map<TblStudentMark>(markObject);

            _dbContext.TblStudentMarks.Add(mappedObject);
            var result = await _dbContext.SaveChangesAsync();

            markObject.Id = mappedObject.Id;
            if (result > 0) return markObject;

            return new ESCHOOLING.Shared.Models.Marks();
        }

        /// <summary>
        /// Gets all active marks.
        /// </summary>
        public async Task<List<ESCHOOLING.Shared.Models.Marks>> GetAllMarksAsync()
        {
            try
            {
                var marks = await _dbContext.TblStudentMarks.AsNoTracking().Where(m => m.IsActive == true).ToListAsync();
                var mappedMarks = _mapper.Map<List<ESCHOOLING.Shared.Models.Marks>>(marks);
                return mappedMarks;
            }
            catch (Exception)
            {
                return new List<ESCHOOLING.Shared.Models.Marks>();
            }
        }
    }
}
