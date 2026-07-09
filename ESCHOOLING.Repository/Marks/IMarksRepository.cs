using ESCHOOLING.Shared.Models;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.StudentMarks
{
    public interface IMarksRepository
    {
        Task<ESCHOOLING.Shared.Models.Marks> AddMarkAsync(ESCHOOLING.Shared.Models.Marks markObject);
    }
}
