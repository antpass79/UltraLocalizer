using AutoMapper;
using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class GroupedStringEntityAdapterService : IJobListService
    {
        private readonly IMapper _mapper;
        private readonly UltraDBEditConcept _ultraDBEditConcept;
        private readonly LocalizationContext _localizationContext;

        public GroupedStringEntityAdapterService(
            IMapper mapper,
            UltraDBEditConcept ultraDBEditConcept,
            LocalizationContext localizationContext)
        {
            _mapper = mapper;
            _ultraDBEditConcept = ultraDBEditConcept;
            _localizationContext = localizationContext;
        }

        async public Task<IEnumerable<JobListConcept>> GetAllAsync(string componentNamespace, string internalNamespace, int languageId, int jobListId)
        {
            var ISOCoding = _localizationContext.LocLanguages.Find(languageId).Isocoding;
            var result = await Task.FromResult(_ultraDBEditConcept.GetGroupledDataBy(componentNamespace, internalNamespace, ISOCoding, jobListId));
            
            return await Task.FromResult(_mapper.Map<IEnumerable<JobListConcept>>(result));
        }

        public Task<IEnumerable<JobListConcept>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<JobListConcept> GetAsync(int key)
        {
            throw new System.NotImplementedException();
        }
    }
}
