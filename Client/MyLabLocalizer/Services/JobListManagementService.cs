using MyLabLocalizer.Models;
using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Core.Extensions;
using MyLabLocalizer.Core.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    class JobListManagementService : IJobListManagementService
    {
        private const string ENDPOINT_READ = "read/";
        private const string ENDPOINT_WRITE = "write/";

        private const string ENDPOINT_NotTranslatedConceptView = "NotTranslatedConceptView";
        private const string ENDPOINT_ComponentNamespaceGroup = "ComponentNamespaceGroup";
        private const string ENDPOINT_JobList = "JobList";
        private const string ENDPOINT_Concept = "Concept";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public JobListManagementService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddress());
        }

        async public Task<IEnumerable<BindableComponentNamespaceGroup>> GetComponentNamespaceGroupsAsync(Language language)
        {
            var result = await _secureHttpClient.SendAsync(HttpMethod.Get, ENDPOINT_READ + ENDPOINT_ComponentNamespaceGroup, language);
            return await result.GetValue<IEnumerable<BindableComponentNamespaceGroup>>();
        }

        async public Task<IEnumerable<BindableNotTranslatedConceptView>> GetNotTranslatedConceptsAsync(BindableComponentNamespace componentNamespace, BindableInternalNamespace internalNamespace, Language language)
        {
            var search = new NotTranslatedConceptViewSearch
            {
                ComponentNamespace = componentNamespace,
                InternalNamespace = internalNamespace,
                Language = language
            };

            var result = await _secureHttpClient.SendAsync(HttpMethod.Get, ENDPOINT_READ + ENDPOINT_NotTranslatedConceptView, search);
            return await result.GetValue<IEnumerable<BindableNotTranslatedConceptView>>();
        }

        async public Task SaveAsync(string jobListName, IEnumerable<BindableNotTranslatedConceptView> notTranslatedConceptViews, ApplicationUser user, Language language)
        {
            var newJobList = new NewJobList
            {
                Name = jobListName,
                User = user,
                Language = language,
                Concepts = notTranslatedConceptViews
            };
            
            await _secureHttpClient.PutAsync(ENDPOINT_WRITE + ENDPOINT_JobList, newJobList);
        }

        async public Task<NewConceptsResult> CheckNewConceptsAsync()
        {
            var result = await _secureHttpClient.PostAsync(ENDPOINT_WRITE + ENDPOINT_Concept, new object());
            return await result.GetValue<NewConceptsResult>();
        }
    }
}
