using Globe.Client.Localizer.Models;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IConceptManagementFiltersService
    {
        Task<IEnumerable<Context>> GetContextAsync();
        Task<IEnumerable<BindableComponentNamespace>> GetComponentNamespacesAsync();
        Task<IEnumerable<BindableInternalNamespace>> GetInternalNamespacesAsync(string componentNamespace);
        Task<IEnumerable<Language>> GetLanguagesAsync();
    }
}
