using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class JobListAdapterService : IAsyncJobListService
    {
        private readonly UltraDBJobList _ultraDBJobList;

        public JobListAdapterService(UltraDBJobList ultraDBJobList)
        {
            _ultraDBJobList = ultraDBJobList;
        }

        public Task DeleteAsync(JobList jobList)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<JobList>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        async public Task<IEnumerable<JobList>> GetAllAsync(string userName, string ISOCoding)
        {
            return await Task.FromResult(_ultraDBJobList.GetAllJobListByUserNameIso(userName, ISOCoding));
        }

        public Task<JobList> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(JobList jobList)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(JobList jobList)
        {
            throw new NotImplementedException();
        }
    }
}
