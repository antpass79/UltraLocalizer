using MyLabLocalizer.Models;
using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Core.Extensions;
using MyLabLocalizer.Core.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    class JobListManagementFiltersService : IJobListManagementFiltersService
    {
        private const string ENDPOINT_JobItem = "JobItem";
        private const string ENDPOINT_Language = "Language";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public JobListManagementFiltersService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddressRead());
        }
        async public Task<IEnumerable<JobItem>> GetJobItemsAsync(string userName, string ISOCoding)
        {
            var search = new JobItemSearch
            {
                UserName = userName,
                ISOCoding = ISOCoding
            };

            var result = await _secureHttpClient.SendAsync(HttpMethod.Get, ENDPOINT_JobItem, search);
            return await result.GetValue<IEnumerable<JobItem>>();
        }

        async public Task<IEnumerable<Language>> GetLanguagesAsync()
        {
            return await _secureHttpClient.GetAsync<IEnumerable<Language>>(ENDPOINT_Language);
        }
    }
}
