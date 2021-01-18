using Globe.Client.Localizer.Models;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IJobListManagementService
    {
        Task<IEnumerable<BindableComponentNamespaceGroup>> GetComponentNamespaceGroupsAsync(Language language);
        Task<IEnumerable<BindableNotTranslatedConceptView>> GetNotTranslatedConceptsAsync(BindableComponentNamespace componentNamespace, BindableInternalNamespace internalNamespace, Language language);
        Task SaveAsync(string jobListName, IEnumerable<BindableNotTranslatedConceptView> notTranslatedConceptViews, ApplicationUser user, Language language);
        Task<NewConceptsResult> CheckNewConceptsAsync();
    }
}
