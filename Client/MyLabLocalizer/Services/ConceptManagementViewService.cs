using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Core.Extensions;
using MyLabLocalizer.Core.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
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

        async public Task<IEnumerable<TranslatedConcept>> GetTranslatedConceptSAsync(ConceptManagementSearch search)
        {
            var result = await _secureHttpClient.SendAsync(HttpMethod.Get, ENDPOINT_ConceptManagementView, search);
            return await result.GetValue<IEnumerable<TranslatedConcept>>();
        }
    }
}
