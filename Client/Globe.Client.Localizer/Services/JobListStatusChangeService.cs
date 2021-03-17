using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class JobListStatusChangeService : IJobListStatusChangeService
    {
        private const string ENDPOINT_WRITE = "write/";

        private const string ENDPOINT_JobListStatus = "JobListStatus";
        
        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public JobListStatusChangeService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddress());
        }

        async public Task SaveAsync(JobList jobList)
        {
            await _secureHttpClient.PutAsync(ENDPOINT_WRITE + ENDPOINT_JobListStatus, jobList);
        }

    }
}
