using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    interface IJobListManagementFiltersService
    {
        Task<IEnumerable<Language>> GetLanguagesAsync();
    }
}
