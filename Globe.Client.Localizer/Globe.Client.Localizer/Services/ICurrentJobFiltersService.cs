using Globe.Client.Localizer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface ICurrentJobFiltersService
    {
        Task<IEnumerable<JobItem>> GetJobItemsAsync(string userName, string ISOCoding);
        Task<IEnumerable<ComponentNamespace>> GetComponentNamespacesAsync();
        Task<IEnumerable<InternalNamespace>> GetInternalNamespacesAsync(string componentNamespace);
        Task<IEnumerable<Language>> GetLanguagesAsync();
    }
}
