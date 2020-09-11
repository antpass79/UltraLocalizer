using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class JobListAdapterService : IAsyncJobItemService
    {
        //private readonly UserManager<GlobeUser> _userManager;
        private readonly IMapper _mapper;
        private readonly UltraDBJobList _ultraDBJobList;

        public JobListAdapterService(/*UserManager<GlobeUser> userManager, */IMapper mapper, UltraDBJobList ultraDBJobList)
        {
            //_userManager = userManager;
            _mapper = mapper;
            _ultraDBJobList = ultraDBJobList;
        }

        public Task DeleteAsync(JobItemDTO jobItem)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<JobItemDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        async public Task<IEnumerable<JobItemDTO>> GetAllAsync(string userName, string ISOCoding)
        {
            //var user = await _userManager.FindByNameAsync(userName);
            //var isMaster =
            //    await _userManager.IsInRoleAsync(user, "Admin") ||
            //    await _userManager.IsInRoleAsync(user, "MasterTranslator");

            var isMaster = true;

            var result = await Task.FromResult(_ultraDBJobList.GetAllJobListByUserNameIso(userName, ISOCoding, isMaster));
            return await Task.FromResult(_mapper.Map<IEnumerable<JobItemDTO>>(result));
        }

        public Task<JobItemDTO> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(JobItemDTO jobItem)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(JobItemDTO jobItem)
        {
            throw new NotImplementedException();
        }
    }
}
