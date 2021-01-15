using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class JobListStatusViewService : IJobListStatusViewService
    {
        //private const string ENDPOINT_JobListView = "JobListView";

        //private readonly IAsyncSecureHttpClient _secureHttpClient;

        public JobListStatusViewService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            //_secureHttpClient = secureHttpClient;
            //_secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddressRead());
        }

        async public Task<IEnumerable<JobList>> GetJobListViewsAsync(JobListSearch search)
        {
            //var result = await _secureHttpClient.SendAsync(HttpMethod.Get, ENDPOINT_JobListView, search);
            var result = new List<JobList>();

            result.Add(new JobList { Id = 213, LanguageId = 1, Name = "MockJobList1", OwnerUserName = "anto", Status = JobListStatus.DRAFT });
            result.Add(new JobList { Id = 31321, LanguageId = 4, Name = "MockJobList4", OwnerUserName = "anto", Status = JobListStatus.SAVED });
            result.Add(new JobList { Id = 1233, LanguageId = 3, Name = "MockJobList3", OwnerUserName = "anto", Status = JobListStatus.ASSIGNED });
            result.Add(new JobList { Id = 645, LanguageId = 1, Name = "MockJobListTest", OwnerUserName = "laura.bigi", Status = JobListStatus.TO_BE_REVISE });

            //return await result.GetValue<IEnumerable<JobList>>();

            return await Task.FromResult(result);
        }
    }
}
