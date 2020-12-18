using AutoMapper;
using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using Globe.TranslationServer.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class XmlGroupedStringEntityService : IAsyncXmlGroupedStringEntityService
    {
        private readonly LocalizationContext _localizationContext;
        private readonly IAsyncXmlDefinitionReaderService _xmlDefinitionReaderService;

        public XmlGroupedStringEntityService(
            LocalizationContext localizationContext,
            IAsyncXmlDefinitionReaderService xmlDefinitionReaderService)
        {
            _localizationContext = localizationContext;
            _xmlDefinitionReaderService = xmlDefinitionReaderService;
        }

        async public Task<IEnumerable<JobListConcept>> GetAllAsync(string componentNamespace, string internalNamespace, int languageId, int jobItemId)
        {
            var items = await _xmlDefinitionReaderService.ReadAsync(Path.Combine(Directory.GetCurrentDirectory(), Constants.XML_FOLDER));
            return await Task.FromResult(Filter(items, componentNamespace, internalNamespace, languageId, jobItemId));
        }

        public Task<IEnumerable<JobListConcept>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<JobListConcept> GetAsync(int key)
        {
            throw new System.NotImplementedException();
        }

        private IEnumerable<JobListConcept> Filter(IEnumerable<JobListConcept> items, string componentNamespace, string internalNamespace, int languageId, int jobItemId)
        {
            //var ISOCoding = _localizationContext.LocLanguages.Find(languageId).Isocoding;
            //var result = items
            //    .AsQueryable()
            //    .Where(item => item.)

            //return result;
            return items;
        }
    }
}
