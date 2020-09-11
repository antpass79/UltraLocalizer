using AutoMapper;
using Globe.TranslationServer.DTOs;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class ConceptViewProxyService : IAsyncConceptViewProxyService
    {
        private readonly IAsyncGroupedStringEntityService _groupedStringEntityService;
        private readonly IAsyncXmlGroupedStringEntityService _xmlGroupedStringEntityService;

        public ConceptViewProxyService(
                    IAsyncGroupedStringEntityService groupedStringEntityService,
                    IAsyncXmlGroupedStringEntityService xmlGroupedStringEntityService)
        {
            _groupedStringEntityService = groupedStringEntityService;
            _xmlGroupedStringEntityService = xmlGroupedStringEntityService;
        }

        async public Task<IEnumerable<ConceptViewDTO>> GetAllAsync(ConceptViewSearchDTO search)
        {
            IEnumerable<ConceptViewDTO> result;

            if (search.WorkingMode == WorkingMode.FromXml)
                result = await _xmlGroupedStringEntityService.GetAllAsync(search.ComponentNamespace, search.InternalNamespace, search.LanguageId, search.JobItemId);
            else
                result = await _groupedStringEntityService.GetAllAsync(search.ComponentNamespace, search.InternalNamespace, search.LanguageId, search.JobItemId);

            return result;
        }
    }
}
