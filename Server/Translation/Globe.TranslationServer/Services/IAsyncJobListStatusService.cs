using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncJobListStatusService
    {
        Task<IEnumerable<JobListDTO>> GetAllAsync();
        Task<IEnumerable<JobListDTO>> GetAsync(int languageId, string userName, string jobListStatus);
    }
}
