using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IAsyncJobListStatusService
    {
        Task<IEnumerable<JobListDTO>> GetAllAsync();
        Task<IEnumerable<JobListDTO>> GetAsync(int languageId, string userName, string jobListStatus);
    }
}
