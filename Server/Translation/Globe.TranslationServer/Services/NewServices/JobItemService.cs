using Globe.BusinessLogic.Repositories;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class JobItemService : IAsyncJobItemService
    {
        private readonly IAsyncReadRepository<LocJobList> _repository;

        public JobItemService(IAsyncReadRepository<LocJobList> repository)
        {
            _repository = repository;
        }

        public Task DeleteAsync(JobItemDTO jobItem)
        {
            throw new System.NotImplementedException();
        }

        async public Task<IEnumerable<JobItemDTO>> GetAllAsync(string userName, string ISOCoding, bool isInAdministratorGroup)
        {      
            var language = ISOCoding.GetLanguage();
            var query = await _repository.QueryAsync();
            var jobs = query
                .WhereIf(item => item.UserName == userName && item.IdisoCoding == (int)language, !isInAdministratorGroup)
                .WhereIf(item => item.IdisoCoding == (int)language, isInAdministratorGroup)
                .Select(item => new JobItemDTO
                {
                    Id = item.Id,
                    Name = isInAdministratorGroup ? $"{item.UserName} - {item.JobName}" : item.JobName,
                    IsoId = (int)language
                })
                .AsEnumerable()
                .OrderBy(item => item.Name)
                .ToList();

            if (isInAdministratorGroup && jobs.Count > 0)
                jobs.Insert(0, new JobItemDTO { Id = 0, IsoId = 0, Name = "all" });

            return await Task.FromResult(jobs);
        }

        //Bisognerebbe utilizzare JobList invece che JobItemDTO
        public async Task<IEnumerable<JobItemDTO>> GetAllAsync()
        {           
            try
            {
                var items = await _repository.QueryAsync();

                //Se utilizzassi il joblistConcept sarebbe sbagliato. in pratica e' un concept
                //invece io devo utilizzare un altro servizio, che mi ritorna una joblist

                var result = items
                    .Select(entity => new JobItemDTO
                    {
                        Id = entity.Id,
                        Name = entity.JobName,
                        //LanguageId = entity.JobListLanguageId,
                        //OwnerUserName = entity.JobListUserName
                    })
                    .Distinct()
                    .ToList();

                var result2 = items.GroupBy(item => new { item.JobName }).Select(x => x.First()).ToList();

                return await Task.FromResult(result);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error during JobItemService.GetAllNamesAsync(), {e.Message}");
            }
            
        }

        public Task<JobItemDTO> GetAsync(int key)
        {
            throw new System.NotImplementedException();
        }

        public Task InsertAsync(JobItemDTO jobItem)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(JobItemDTO jobItem)
        {
            throw new System.NotImplementedException();
        }
    }
}
