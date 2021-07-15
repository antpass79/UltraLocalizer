using MyLabLocalizer.LocalizationService.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IAsyncJobItemService : IAsyncService<JobItemDTO>
    {
        Task<IEnumerable<JobItemDTO>> GetAllAsync(string userName, string ISOCoding, bool isInAdministratorGroup);
    }
}
