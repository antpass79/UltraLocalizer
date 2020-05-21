using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class CurrentJobConceptViewsService : ICurrentJobConceptViewsService
    {
        private const string ENDPOINT_ConceptView = "ConceptView";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public CurrentJobConceptViewsService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddress());
        }

        async public Task<IEnumerable<ConceptView>> GetConceptViewsAsync(ConceptViewSearch search)
        {
            var result = await _secureHttpClient.SendAsync<ConceptViewSearch>(HttpMethod.Get, ENDPOINT_ConceptView, search);
            return await result.GetValue<IEnumerable<ConceptView>>();
        }
    }
}
