using System.Threading.Tasks;

namespace ESCHOOLING.Shared
{
    public interface IMarksService
    {
        Task<ESCHOOLING.Shared.Models.Marks> SaveMarkAsync(ESCHOOLING.Shared.Models.Marks markObject);
    }
}
