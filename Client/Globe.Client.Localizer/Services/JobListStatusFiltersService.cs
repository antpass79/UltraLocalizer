using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Services;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Globe.Client.Localizer.Services
{
    //Fake Service
    class JobListStatusFiltersService : IJobListStatusFiltersService
    {
        private const string ENDPOINT_Language = "Language";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public JobListStatusFiltersService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddressRead());
        }

        public Task<IEnumerable<BindableJobListStatus>> GetJobListStatusesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BindableApplicationUser>> GetApplicationUsersAsync()
        {
            throw new NotImplementedException();
        }
     
        public Task<IEnumerable<JobItem>> GetJobListsAsync(string userName)
        {
            throw new NotImplementedException();
        }

        async public Task<IEnumerable<Language>> GetLanguagesAsync()
        {
            var languages = (await _secureHttpClient.GetAsync<IEnumerable<Language>>(ENDPOINT_Language)).ToList();

            languages.Insert(0, new Language
            {
                Id = 0,
                IsoCoding = "all",
                Description = "all",
                Name = "all"
            }) ;

            return await Task.FromResult(languages);
        }
    }
}
