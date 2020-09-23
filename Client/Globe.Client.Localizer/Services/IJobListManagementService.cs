using Globe.Client.Localizer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IJobListManagementService
    {
        Task<IEnumerable<InternalNamespaceGroup>> GetInternalNamespaceGroupsAsync(Language language);
        Task<IEnumerable<NotTranslatedConceptView>> GetNotTranslatedConceptsAsync(ComponentNamespace componentNamespace, InternalNamespace internalNamespace, Language language);
        Task SaveAsync(string jobListName, IEnumerable<NotTranslatedConceptView> notTranslatedConceptViews, ApplicationUser user, Language language);
    }
}
