using AutoMapper;
using Globe.TranslationServer.DTOs;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class ConceptViewProxyService : IAsyncConceptViewProxyService
    {
        const string XML_FOLDER = "XmlDefinitions";

        private readonly IMapper _mapper;
        private readonly IAsyncGroupedStringEntityService _groupedStringEntityService;
        private readonly IAsyncXmlDefinitionReaderService _xmlDefinitionReaderService;

        public ConceptViewProxyService(
                    IMapper mapper,
                    IAsyncGroupedStringEntityService groupedStringEntityService,
                    IAsyncXmlDefinitionReaderService xmlDefinitionReaderService)
        {
            _mapper = mapper;
            _groupedStringEntityService = groupedStringEntityService;
            _xmlDefinitionReaderService = xmlDefinitionReaderService;
        }

        async public Task<IEnumerable<ConceptViewDTO>> GetAllAsync(ConceptViewSearchDTO search)
        {
            IEnumerable<ConceptViewDTO> result;

            if (search.WorkingMode == WorkingMode.FromXml)
                result = await _xmlDefinitionReaderService.ReadAsync(Path.Combine(Directory.GetCurrentDirectory(), XML_FOLDER));
            else
            {
                var items = await _groupedStringEntityService.GetAllAsync(search.ComponentNamespace, search.InternalNamespace, search.ISOCoding, search.JobListId);
                result = _mapper.Map<IEnumerable<ConceptViewDTO>>(items);
            }

            return result;
        }
    }
}
