using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class JobListStatusViewService : IJobListStatusViewService
    {
        private const string ENDPOINT_JobListView = "JobListView";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public JobListStatusViewService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddressRead());
        }

        async public Task<IEnumerable<JobList>> GetJobListViewsAsync(JobListSearch search)
        {
            var result = await _secureHttpClient.SendAsync(HttpMethod.Get, ENDPOINT_JobListView, search);
            var jobLists = await result.GetValue<IEnumerable<JobListDTO>>();

            return jobLists.Select(item => new JobList
            {
                Id = item.Id,
                Language = item.Language,
                LanguageId = item.LanguageId,
                Name = item.Name,
                Status = item.Status,
                OwnerUserName = item.OwnerUserName,
                TotalConcepts = item.TotalConcepts,
                NumberTranslations = item.NumberTranslations,
                NumberTranslationsDraft = item.NumberTranslationsDraft
                //NextStatus =
                //Altri stati come percentuali ecc vengono creati automaticamente alla creazione dell'istanza della classe
            });
        }
    }
}
