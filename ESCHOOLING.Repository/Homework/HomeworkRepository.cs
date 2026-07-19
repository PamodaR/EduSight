using AutoMapper;
using ESCHOOLING.DataAccess.EntityModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.StudentHomework
{
    public class HomeworkRepository : IHomeworkRepository
    {
        private readonly ECOM_WebContext _dbContext;
        private readonly IMapper _mapper;

        public HomeworkRepository(ECOM_WebContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ESCHOOLING.Shared.Models.Homework> AddHomeworkAsync(ESCHOOLING.Shared.Models.Homework homeworkObject)
        {
            try
            {
                var mappedObject = _mapper.Map<TblHomework>(homeworkObject);

                _dbContext.TblHomeworks.Add(mappedObject);
                var result = await _dbContext.SaveChangesAsync();

                homeworkObject.Id = mappedObject.Id;
                if (result > 0) return homeworkObject;

                return new ESCHOOLING.Shared.Models.Homework();
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
                var homework = await _dbContext.TblHomeworks.AsNoTracking().Where(h => h.IsActive == true).ToListAsync();
                var mappedHomework = _mapper.Map<List<ESCHOOLING.Shared.Models.Homework>>(homework);
                return mappedHomework;
            }
            catch (Exception)
            {
                return new List<ESCHOOLING.Shared.Models.Homework>();
            }
        }
    }
}
