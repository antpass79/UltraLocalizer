using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class XmlGroupedStringEntityService : IAsyncXmlGroupedStringEntityService
    {
        const string XML_FOLDER = "XmlDefinitions";

        private readonly LocalizationContext _localizationContext;
        private readonly IAsyncXmlDefinitionReaderService _xmlDefinitionReaderService;

        public XmlGroupedStringEntityService(
            LocalizationContext localizationContext,
            IAsyncXmlDefinitionReaderService xmlDefinitionReaderService)
        {
            _localizationContext = localizationContext;
            _xmlDefinitionReaderService = xmlDefinitionReaderService;
        }

        async public Task<IEnumerable<ConceptViewDTO>> GetAllAsync(string componentNamespace, string internalNamespace, int languageId, int jobItemId)
        {
            var items = await _xmlDefinitionReaderService.ReadAsync(Path.Combine(Directory.GetCurrentDirectory(), XML_FOLDER));
            return await Task.FromResult(Filter(items, componentNamespace, internalNamespace, languageId, jobItemId));
        }

        public Task<IEnumerable<ConceptViewDTO>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<ConceptViewDTO> GetAsync(int key)
        {
            throw new System.NotImplementedException();
        }

        private IEnumerable<ConceptViewDTO> Filter(IEnumerable<ConceptViewDTO> items, string componentNamespace, string internalNamespace, int languageId, int jobItemId)
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
