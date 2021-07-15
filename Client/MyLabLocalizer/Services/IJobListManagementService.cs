using MyLabLocalizer.Models;
using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    interface IJobListManagementService
    {
        Task<IEnumerable<BindableComponentNamespaceGroup>> GetComponentNamespaceGroupsAsync(Language language);
        Task<IEnumerable<BindableNotTranslatedConceptView>> GetNotTranslatedConceptsAsync(BindableComponentNamespace componentNamespace, BindableInternalNamespace internalNamespace, Language language);
        Task SaveAsync(string jobListName, IEnumerable<BindableNotTranslatedConceptView> notTranslatedConceptViews, ApplicationUser user, Language language);
        Task<NewConceptsResult> CheckNewConceptsAsync();
    }
}
