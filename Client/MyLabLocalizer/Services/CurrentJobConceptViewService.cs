using MyLabLocalizer.Models;
using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Core.Extensions;
using MyLabLocalizer.Core.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
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

        async public Task<IEnumerable<JobListConcept>> GetConceptViewsAsync(JobListConceptSearch search)
        {
            var result = await _secureHttpClient.SendAsync(HttpMethod.Get, ENDPOINT_ConceptView, search);
            return await result.GetValue<IEnumerable<JobListConcept>>();
        }

        async public Task<ConceptDetails> GetConceptDetailsAsync(JobListConcept jobListConcept)
        {
            var result = await _secureHttpClient.SendAsync<JobListConcept>(HttpMethod.Get, jobListConcept.DetailsLink, jobListConcept);
            return await result.GetValue<ConceptDetails>();
        }
    }
}
