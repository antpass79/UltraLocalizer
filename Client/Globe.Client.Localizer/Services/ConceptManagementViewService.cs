using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class ConceptManagementViewService : IConceptManagementViewService
    {
        private const string ENDPOINT_ConceptManagementView = "ConceptManagementView";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public ConceptManagementViewService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddressRead());
        }

        async public Task<IEnumerable<ConceptTranslated>> GetConceptViewsAsync(ConceptManagementSearch search)
        {
            var result = await _secureHttpClient.SendAsync(HttpMethod.Get, ENDPOINT_ConceptManagementView, search);
            return await result.GetValue<IEnumerable<ConceptTranslated>>();
        }
    }
}
