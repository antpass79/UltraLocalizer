using Globe.Client.Localizer.Models;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface ICurrentJobFiltersService
    {
        Task<IEnumerable<JobItem>> GetJobItemsAsync(string userName, string ISOCoding);
        Task<IEnumerable<BindableComponentNamespace>> GetComponentNamespacesAsync();
        Task<IEnumerable<Globe.Client.Localizer.Models.BindableInternalNamespace>> GetInternalNamespacesAsync(string componentNamespace);
        Task<IEnumerable<Language>> GetLanguagesAsync();
    }
}
