using MyLabLocalizer.Models;
using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    interface ICurrentJobFiltersService
    {
        Task<IEnumerable<JobItem>> GetJobItemsAsync(string userName, string ISOCoding);
        Task<IEnumerable<BindableComponentNamespace>> GetComponentNamespacesAsync();
        Task<IEnumerable<BindableInternalNamespace>> GetInternalNamespacesAsync(string componentNamespace);
        Task<IEnumerable<Language>> GetLanguagesAsync();
    }
}
