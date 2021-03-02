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
            var joblists = new List<JobList>();

            joblists.Add(new JobList 
            { 
                Id = 213, 
                LanguageId = 1, 
                Name = "MockJobList1", 
                OwnerUserName = "anto", 
                Status = new JobListStatus { Description = "Assigned" },//Status = JobListStatus.Assigned,
                TotalConcepts = 1612,
                NumberTranslations = 0,
                NumberTranslationsDraft = 0              
            });
            joblists.Add(new JobList
            {
                Id = 31321,
                LanguageId = 4,
                Name = "MockJobList2",
                OwnerUserName = "anto",
                Status = new JobListStatus { Description = "Saved" }, //JobListStatus.Saved,
                TotalConcepts = 3320,
                NumberTranslations = 3320,
                NumberTranslationsDraft = 0
            });
            joblists.Add(new JobList
            {
                Id = 1233,
                LanguageId = 3,
                Name = "MockJobList3",
                OwnerUserName = "anto",
                Status = new JobListStatus { Description = "ToBeRevised" },//JobListStatus.ToBeRevised,
                TotalConcepts = 80,
                NumberTranslations = 0,
                NumberTranslationsDraft = 80
            });
            joblists.Add(new JobList
            {
                Id = 645,
                LanguageId = 1,
                Name = "MockJobList4",
                OwnerUserName = "laura.bigi",
                Status = new JobListStatus { Description = "Closed" },//JobListStatus.Closed,
                TotalConcepts = 250,
                NumberTranslations = 250,
                NumberTranslationsDraft = 0
            });

            //questa where lo fara' il servizio sul Server, passadogli la search
            //var result = joblists.Where(item => item.Status.Description == search.JobListStatus && item.OwnerUserName == search.UserName && item.LanguageId == search.LanguageId);

            //return await result.GetValue<IEnumerable<JobList>>();

            return await Task.FromResult(joblists);
        }
    }
}
