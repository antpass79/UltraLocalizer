using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Services;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Globe.Client.Localizer.Services
{
    class ConceptManagementFiltersService : IConceptManagementFiltersService
    {
        private const string ENDPOINT_ComponentNamespace = "ConceptManagementComponentNamespace";
        private const string ENDPOINT_InternalNamespace = "ConceptManagementInternalNamespace";
        private const string ENDPOINT_Context = "Context";
        private const string ENDPOINT_Language = "Language";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public ConceptManagementFiltersService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddressRead());
        }

        async public Task<IEnumerable<BindableComponentNamespace>> GetComponentNamespacesAsync()
        {
            return await _secureHttpClient.GetAsync<IEnumerable<BindableComponentNamespace>>(ENDPOINT_ComponentNamespace);
        }

        async public Task<IEnumerable<BindableInternalNamespace>> GetInternalNamespacesAsync(string componentNamespace)
        {
            return await _secureHttpClient.GetAsync<IEnumerable<BindableInternalNamespace>>(ENDPOINT_InternalNamespace + "/?componentNamespace=" + componentNamespace);
        }

        async public Task<IEnumerable<Context>> GetContextAsync()
        {
            var contexts = (await _secureHttpClient.GetAsync<IEnumerable<Context>>(ENDPOINT_Context)).ToList();

            contexts.Insert(0, new Context
            {
                Name = "all"              
            });

            return await Task.FromResult(contexts);

        }

        async public Task<IEnumerable<Language>> GetLanguagesAsync()
        {
            var languages = (await _secureHttpClient.GetAsync<IEnumerable<Language>>(ENDPOINT_Language)).ToList();

            languages.Insert(0, new Language
            {
                Id = 0,
                IsoCoding = "all",
                Description = "all",
                Name = "all"
            }) ;

            return await Task.FromResult(languages);
        }
    }
}
