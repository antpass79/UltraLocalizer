using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class CurrentJobConceptViewService : ICurrentJobConceptViewService
    {
        private const string ENDPOINT_ConceptView = "ConceptView";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public CurrentJobConceptViewService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddressRead());
        }

        async public Task<IEnumerable<ConceptView>> GetConceptViewsAsync(ConceptViewSearch search)
        {
            var result = await _secureHttpClient.SendAsync<ConceptViewSearch>(HttpMethod.Get, ENDPOINT_ConceptView, search);
            return await result.GetValue<IEnumerable<ConceptView>>();
        }

        async public Task<ConceptDetails> GetConceptDetailsAsync(ConceptView concept)
        {
            var result = await _secureHttpClient.SendAsync<ConceptView>(HttpMethod.Get, concept.DetailsLink, concept);
            return await result.GetValue<ConceptDetails>();
        }
    }
}
