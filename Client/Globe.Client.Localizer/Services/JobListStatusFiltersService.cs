using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Services;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Net.Http;
using Globe.Client.Platform.Extensions;
using Globe.Shared.Utilities;

namespace Globe.Client.Localizer.Services
{
    //Fake Service
    class JobListStatusFiltersService : IJobListStatusFiltersService
    {
        private const string ENDPOINT_Language = "Language";

        private readonly IAsyncSecureHttpClient _secureHttpClient;
        private readonly IUserService _userService;

        public JobListStatusFiltersService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService, IUserService userService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddressRead());

            _userService = userService;
        }

        public Task<IEnumerable<BindableJobListStatus>> GetJobListStatusesAsync()
        {
            //return await _secureHttpClient.GetAsync<IEnumerable<BindableJobListStatus>>(ENDPOINT_JobListStatus);

            var jobListStatuses = new List<BindableJobListStatus>();
            jobListStatuses.Add(new BindableJobListStatus { Description = "Assigned" });
            jobListStatuses.Add(new BindableJobListStatus { Description = "Closed" });
            jobListStatuses.Add(new BindableJobListStatus { Description = "ToBeRevised" });
            jobListStatuses.Add(new BindableJobListStatus { Description = "Saved" });
            jobListStatuses.Add(new BindableJobListStatus { Description = "Deleted" });

            jobListStatuses.Insert(0, new BindableJobListStatus
            {
                Description = "all"
            });

            return Task.FromResult(jobListStatuses.AsEnumerable<BindableJobListStatus>());
        }

        public async Task<IEnumerable<BindableApplicationUser>> GetApplicationUsersAsync(string userName)
        {
            var users = (await _userService.GetUsersAsync(userName))             
               .Select(item => new BindableApplicationUser
               {
                   Id = item.Id,
                   FirstName = item.FirstName,
                   LastName = item.LastName,
                   UserName = item.UserName,
                   Email = item.Email
               })
               .ToList();

            if(users.Count() > 1)
            {
                users.Insert(0, new BindableApplicationUser
                {
                    Id = "0",
                    UserName = "all"
                });
            }

            return await Task.FromResult(users);
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
