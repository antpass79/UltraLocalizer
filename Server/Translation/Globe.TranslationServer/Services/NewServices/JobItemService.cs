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
        private readonly IReadRepository<LocLanguage> _languageRepository;

        public JobItemService(
            IAsyncReadRepository<LocJobList> repository, 
            IReadRepository<LocLanguage> languageRepository)
        {
            _repository = repository;
            _languageRepository = languageRepository;
        }

        public Task DeleteAsync(JobItemDTO jobItem)
        {
            throw new System.NotImplementedException();
        }

        async public Task<IEnumerable<JobItemDTO>> GetAllAsync(string userName, string ISOCoding, bool isInAdministratorGroup)
        {
            //var language = ISOCoding.GetLanguage();
            var languageId = GetLanguageId(ISOCoding);
            var query = await _repository.QueryAsync();
            var jobs = query
                .WhereIf(item => item.UserName == userName && item.IdisoCoding == languageId, !isInAdministratorGroup)
                .WhereIf(item => item.IdisoCoding == languageId, isInAdministratorGroup)
                .Select(item => new JobItemDTO
                {
                    Id = item.Id,
                    Name = isInAdministratorGroup ? $"{item.UserName} - {item.JobName}" : item.JobName,
                    IsoId = languageId
                })
                .AsEnumerable()
                .OrderBy(item => item.Name)
                .ToList();

            return await Task.FromResult(jobs);
        }

        public async Task<IEnumerable<JobItemDTO>> GetAllAsync()
        {           
            try
            {
                var items = await _repository.QueryAsync();

                var result = items
                    .Select(entity => new JobItemDTO
                    {
                        Id = entity.Id,
                        Name = entity.JobName
                    })
                    .Distinct()
                    .ToList();

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

        private int GetLanguageId(string isoCoding)
        {
            var query = _languageRepository
                .Get()
                .Where(item => item.Isocoding == isoCoding)
                .Single().Id;


            return query;         
        }
    }
}
