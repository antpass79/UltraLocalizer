using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class JobListAdapterService : IAsyncJobListService
    {
        //private readonly UserManager<GlobeUser> _userManager;
        private readonly UltraDBJobList _ultraDBJobList;

        public JobListAdapterService(/*UserManager<GlobeUser> userManager, */UltraDBJobList ultraDBJobList)
        {
            //_userManager = userManager;
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
            //var user = await _userManager.FindByNameAsync(userName);
            //var isMaster =
            //    await _userManager.IsInRoleAsync(user, "Admin") ||
            //    await _userManager.IsInRoleAsync(user, "MasterTranslator");

            var isMaster = true;

            return await Task.FromResult(_ultraDBJobList.GetAllJobListByUserNameIso(userName, ISOCoding, isMaster));
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
