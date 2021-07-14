using Globe.Shared.DTOs;
using Globe.TranslationServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class JobListStatusService : IAsyncJobListStatusService
    {
        private readonly LocalizationContext _localizationContext;

        public JobListStatusService(LocalizationContext localizationContext)
        {
            _localizationContext = localizationContext;
        }

        //TODO: Mocked
        async public Task<IEnumerable<JobListDTO>> GetAllAsync()
        {
            //var items = _localizationContext.VJobListConcepts
            //    .Select(item => new JobListDTO
            //    { 


            //    }
            //    );
            
            var joblists = new List<JobListDTO>();

            //RICORDA nuova Lista da utilizzare sara vJobList (con tutti i conteggi necessari)!

            joblists.Add(new JobListDTO
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
            joblists.Add(new JobListDTO
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
            joblists.Add(new JobListDTO
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
            joblists.Add(new JobListDTO
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
            joblists.Add(new JobListDTO
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
            joblists.Add(new JobListDTO
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
            joblists.Add(new JobListDTO
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

            return await Task.FromResult(joblists);
        }

        async public Task<IEnumerable<JobListDTO>> GetAsync(int languageId, string userName, string jobListStatus)
        {
            //TODO: Mocked version.
            return await GetAllAsync();

            //Far andare script VJobList (scaffholding)
            //var results = _localizationContext.VJobList
            //    .Where(item => item.Status.Description == jobListStatus && 
            //    item.OwnerUserName == userName && 
            //    item.LanguageId == languageId);
        }
    }
}
