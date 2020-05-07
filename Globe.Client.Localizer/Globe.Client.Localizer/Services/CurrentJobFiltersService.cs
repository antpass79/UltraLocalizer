using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class CurrentJobFiltersService : ICurrentJobFiltersService
    {
        private const string ENDPOINT_ComponentNamespace = "ComponentNamespace";
        private const string ENDPOINT_InternalNamespace = "InternalNamespace";
        private const string ENDPOINT_JobItem = "JobItem";
        private const string ENDPOINT_Language = "Language";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public CurrentJobFiltersService(IAsyncSecureHttpClient secureHttpClient)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(ConfigurationManager.AppSettings["LocalizableStringBaseAddress"]);
        }

        async public Task<IEnumerable<ComponentNamespace>> GetComponentNamespacesAsync()
        {
            return await _secureHttpClient.GetAsync<IEnumerable<ComponentNamespace>>(ENDPOINT_ComponentNamespace);
        }

        async public Task<IEnumerable<InternalNamespace>> GetInternalNamespacesAsync(string componentNamespace)
        {
            return await _secureHttpClient.GetAsync<IEnumerable<InternalNamespace>>(ENDPOINT_InternalNamespace + "/?componentNamespace=" + componentNamespace);
        }

        async public Task<IEnumerable<JobItem>> GetJobItemsAsync(string userName, string ISOCoding)
        {
            var search = new JobItemSearch
            {
                UserName = userName,
                ISOCoding = ISOCoding
            };

            var result = await _secureHttpClient.SendAsync<JobItemSearch>(HttpMethod.Get, ENDPOINT_JobItem, search);
            return await result.GetValue<IEnumerable<JobItem>>();
        }

        async public Task<IEnumerable<Language>> GetLanguagesAsync()
        {
            return await _secureHttpClient.GetAsync<IEnumerable<Language>>(ENDPOINT_Language);
        }
    }
}
