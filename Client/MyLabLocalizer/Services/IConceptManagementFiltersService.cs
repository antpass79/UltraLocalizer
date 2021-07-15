using MyLabLocalizer.Models;
using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    interface IConceptManagementFiltersService
    {
        Task<IEnumerable<Context>> GetContextAsync();
        Task<IEnumerable<BindableComponentNamespace>> GetComponentNamespacesAsync();
        Task<IEnumerable<BindableInternalNamespace>> GetInternalNamespacesAsync(string componentNamespace);
        Task<IEnumerable<Language>> GetLanguagesAsync();
    }
}
