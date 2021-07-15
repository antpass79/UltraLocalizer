using MyLabLocalizer.Models;
using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    interface IJobListStatusFiltersService
    {
        Task<IEnumerable<BindableJobListStatus>> GetJobListStatusesAsync();
        Task<IEnumerable<BindableApplicationUser>> GetApplicationUsersAsync(string userName);
        Task<IEnumerable<Language>> GetLanguagesAsync();
    }
}
