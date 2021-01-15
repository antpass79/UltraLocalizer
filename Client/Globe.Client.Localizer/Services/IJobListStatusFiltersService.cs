using Globe.Client.Localizer.Models;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IJobListStatusFiltersService
    {
        Task<IEnumerable<BindableJobListStatus>> GetJobListStatusesAsync();
        Task<IEnumerable<BindableApplicationUser>> GetApplicationUsersAsync();
        Task<IEnumerable<Language>> GetLanguagesAsync();
        Task<IEnumerable<JobItem>> GetJobListsAsync(string userName);
    }
}
