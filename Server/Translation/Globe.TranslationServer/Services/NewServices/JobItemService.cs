using Globe.BusinessLogic.Repositories;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Extensions;
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

            return await Task.FromResult(jobs);
        }

        public Task<IEnumerable<JobItemDTO>> GetAllAsync()
        {
            throw new System.NotImplementedException();
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
