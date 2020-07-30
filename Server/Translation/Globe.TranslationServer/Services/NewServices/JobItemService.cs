using Globe.BusinessLogic.Repositories;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Extensions;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class JobItemService : IAsyncJobListService
    {
        private readonly IAsyncReadRepository<LocJobList> _repository;

        public JobItemService(IAsyncReadRepository<LocJobList> repository)
        {
            _repository = repository;
        }

        public Task DeleteAsync(JobList job2Concept)
        {
            throw new System.NotImplementedException();
        }

        async public Task<IEnumerable<JobList>> GetAllAsync(string userName, string ISOCoding)
        {
            //bool l_isMaster = (Roles.IsUserInRole(UserName, "MasterTranslator") ||
            //                  Roles.IsUserInRole(UserName, "Admin"));

            //var user = await userManager.FindByNameAsync(UserName);
            //bool l_isMaster = await userManager.IsInRoleAsync(user, "MasterTranslator") || await userManager.IsInRoleAsync(user, "Admin");

            // ANTO check roles before
            var isMaster = true;

            var language = ISOCoding.GetLanguage();
            var query = await _repository.QueryAsync();
            var jobs = query
                .WhereIf(item => item.UserName == userName && item.IdisoCoding == (int)language, !isMaster)
                .WhereIf(item => item.IdisoCoding == (int)language, isMaster)
                .Select(item => new JobList
                {
                    IDJob = item.Id,
                    JobName = isMaster ? $"{item.UserName} - {item.JobName}" : item.JobName,
                    IDIso = (int)language
                })
                .AsEnumerable()
                .OrderBy(item => item.JobName);

            return await Task.FromResult(jobs);
        }

        public Task<IEnumerable<JobList>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<JobList> GetAsync(int key)
        {
            throw new System.NotImplementedException();
        }

        public Task InsertAsync(JobList job2Concept)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(JobList job2Concept)
        {
            throw new System.NotImplementedException();
        }
    }
}
