using MyLabLocalizer.Models;
using MyLabLocalizer.Core.Services;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
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
