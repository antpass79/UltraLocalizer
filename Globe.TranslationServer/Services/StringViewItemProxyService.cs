using AutoMapper;
using Globe.TranslationServer.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class StringViewItemProxyService : IAsyncStringViewItemProxyService
    {
        const string XML_FOLDER = "XmlDefinitions";

        private readonly IMapper _mapper;
        private readonly IAsyncGroupedStringEntityService _groupedStringEntityService;
        private readonly IAsyncXmlDefinitionReaderService _xmlDefinitionReaderService;

        public StringViewItemProxyService(
                    IMapper mapper,
                    IAsyncGroupedStringEntityService groupedStringEntityService,
                    IAsyncXmlDefinitionReaderService xmlDefinitionReaderService)
        {
            _mapper = mapper;
            _groupedStringEntityService = groupedStringEntityService;
            _xmlDefinitionReaderService = xmlDefinitionReaderService;
        }

        async public Task<IEnumerable<StringViewItemDTO>> GetAllAsync(StringViewItemSearchDTO search)
        {
            IEnumerable<StringViewItemDTO> result;

            if (search.WorkingMode == WorkingMode.FromXml)
                result = await _xmlDefinitionReaderService.ReadAsync(Path.Combine(Directory.GetCurrentDirectory(), XML_FOLDER));
            else
            {
                var items = await _groupedStringEntityService.GetAllAsync(search.ComponentNamespace, search.InternalNamespace, search.ISOCoding, search.JobListId);
                result = _mapper.Map<IEnumerable<StringViewItemDTO>>(items);
            }

            return result;
        }
    }
}
