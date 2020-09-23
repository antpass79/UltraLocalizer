using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class JobListManagementService : IJobListManagementService
    {
        private const string ENDPOINT_READ = "read/";
        private const string ENDPOINT_WRITE = "write/";

        private const string ENDPOINT_NotTranslatedConceptView = "NotTranslatedConceptView";
        private const string ENDPOINT_InternalNamespaceGroup = "InternalNamespaceGroup";
        private const string ENDPOINT_JobList = "JobList";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public JobListManagementService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddress());
        }

        async public Task<IEnumerable<InternalNamespaceGroup>> GetInternalNamespaceGroupsAsync(Language language)
        {
            var result = await _secureHttpClient.SendAsync<Language>(HttpMethod.Get, ENDPOINT_READ + ENDPOINT_InternalNamespaceGroup, language);
            return await result.GetValue<IEnumerable<InternalNamespaceGroup>>();
        }

        async public Task<IEnumerable<NotTranslatedConceptView>> GetNotTranslatedConceptsAsync(ComponentNamespace componentNamespace, InternalNamespace internalNamespace, Language language)
        {
            var search = new NotTranslatedConceptViewSearch
            {
                ComponentNamespace = componentNamespace,
                InternalNamespace = internalNamespace,
                Language = language
            };

            var result = await _secureHttpClient.SendAsync<NotTranslatedConceptViewSearch>(HttpMethod.Get, ENDPOINT_READ + ENDPOINT_NotTranslatedConceptView, search);
            return await result.GetValue<IEnumerable<NotTranslatedConceptView>>();
        }

        async public Task SaveAsync(string jobListName, IEnumerable<NotTranslatedConceptView> notTranslatedConceptViews, ApplicationUser user, Language language)
        {
            var savableJobList = new SavableJobList(jobListName, notTranslatedConceptViews, user, language);
            
            await _secureHttpClient.PutAsync<SavableJobList>(ENDPOINT_WRITE + ENDPOINT_JobList, savableJobList);
        }
    }
}
