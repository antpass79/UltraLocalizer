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

            //RICORDA nuova Lista da utilizzare sara vJobList (con tutti i conteggi necessari)!

            joblists.Add(new JobList
            { 
                Id = 213, 
                LanguageId = 1,
                Language = "English",
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
                Language = "English",
                Name = "MockJobList2",
                OwnerUserName = "anto",
                Status = new JobListStatus { Description = "Saved" },
                TotalConcepts = 3320,
                NumberTranslations = 3320,
                NumberTranslationsDraft = 0
            });
            joblists.Add(new JobList
            {
                Id = 1233,
                LanguageId = 3,
                Language = "Italian",
                Name = "MockJobList3",
                OwnerUserName = "anto",
                Status = new JobListStatus { Description = "ToBeRevised" },
                TotalConcepts = 80,
                NumberTranslations = 0,
                NumberTranslationsDraft = 80
            });
            joblists.Add(new JobList
            {
                Id = 645,
                LanguageId = 5,
                Language = "Russian",
                Name = "MockJobList4",
                OwnerUserName = "matteo",
                Status = new JobListStatus { Description = "Closed" },
                TotalConcepts = 250,
                NumberTranslations = 250,
                NumberTranslationsDraft = 0
            });
            joblists.Add(new JobList
            {
                Id = 234,
                LanguageId = 1,
                Language = "French",
                Name = "MockJobList5",
                OwnerUserName = "matteo",
                Status = new JobListStatus { Description = "Assigned" },
                TotalConcepts = 370,
                NumberTranslations = 0,
                NumberTranslationsDraft = 27
            });
            joblists.Add(new JobList
            {
                Id = 777,
                LanguageId = 5,
                Language = "Russian",
                Name = "Delitto_e_castigo",
                OwnerUserName = "fedor.dostoevskij",
                Status = new JobListStatus { Description = "Assigned" },
                TotalConcepts = 1111,
                NumberTranslations = 0,
                NumberTranslationsDraft = 1111
            });
            joblists.Add(new JobList
            {
                Id = 3394,
                LanguageId = 5,
                Language = "Spanish",
                Name = "Breast",
                OwnerUserName = "laura.bigi",
                Status = new JobListStatus { Description = "Assigned" },
                TotalConcepts = 15,
                NumberTranslations = 0,
                NumberTranslationsDraft = 8
            });

            //questa where lo fara' il servizio sul Server, passadogli la search
            //var result = joblists.Where(item => item.Status.Description == search.JobListStatus && item.OwnerUserName == search.UserName && item.LanguageId == search.LanguageId);

            //return await result.GetValue<IEnumerable<JobList>>();

            return await Task.FromResult(joblists);
        }
    }
}
