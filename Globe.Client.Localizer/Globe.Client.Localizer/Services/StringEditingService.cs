using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class StringEditingService : IStringEditingService
    {
        private const string ENDPOINT_ConceptViewItem = "ConceptViewItem";
        private const string ENDPOINT_Context = "Context";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public StringEditingService(IAsyncSecureHttpClient secureHttpClient)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(ConfigurationManager.AppSettings["LocalizableStringBaseAddress"]);
        }

        async public Task<IEnumerable<ConceptViewItem>> GetConceptViewItemsAsync(ConceptViewItemSearch search)
        {
            var result = await _secureHttpClient.SendAsync<ConceptViewItemSearch>(HttpMethod.Get, ENDPOINT_ConceptViewItem, search);
            return await result.GetValue<IEnumerable<ConceptViewItem>>();
        }

        async public Task<IEnumerable<Context>> GetContextsAsync()
        {
            return await _secureHttpClient.GetAsync<IEnumerable<Context>>(ENDPOINT_Context);
        }
    }
}
