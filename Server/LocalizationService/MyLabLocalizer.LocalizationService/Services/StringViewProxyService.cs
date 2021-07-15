using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.DTOs;
using MyLabLocalizer.LocalizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public class StringViewProxyService : IAsyncStringViewProxyService
    {
        private readonly IAsyncConceptSearchService _conceptSearchService;

        public StringViewProxyService(IAsyncConceptSearchService conceptSearchService)
        {
            _conceptSearchService = conceptSearchService;
        }

        async public Task<IEnumerable<StringViewDTO>> GetAllAsync(StringViewSearchDTO search)
        {
            IEnumerable<ConceptSearch> items;

            if (search.SearchBy == ConceptSearchBy.Concept)
            {
                items = search.FilterBy switch
                {
                    ConceptFilterBy.None => await _conceptSearchService.GetSearchConceptbyISO(search.ISOCoding, search.StringValue, true),
                    ConceptFilterBy.Context => await _conceptSearchService.GetSearchConceptbyISObyContext(search.ISOCoding, search.StringValue, search.Context, true),
                    ConceptFilterBy.StringType => await _conceptSearchService.GetSearchConceptbyISObyStringType(search.ISOCoding, search.StringValue, search.StringType.ToString(), true),
                    _ => new List<ConceptSearch>()
                };
            }
            else
            {
                items = search.FilterBy switch
                {
                    ConceptFilterBy.None => await _conceptSearchService.GetSearchStringbyISO(search.ISOCoding, search.StringValue, true),
                    ConceptFilterBy.Context => await _conceptSearchService.GetSearchStringtbyISObyContext(search.ISOCoding, search.StringValue, search.Context, true),
                    ConceptFilterBy.StringType => await _conceptSearchService.GetSearchStringtbyISObyStringType(search.ISOCoding, search.StringValue, search.StringType.ToString(), true),
                    _ => new List<ConceptSearch>()
                };
            }

            var result = items.Select(item =>
            {
                return new StringViewDTO
                {
                    ComponentNamespace = item.ComponentNamespace,
                    Concept = item.Concept,
                    Context = item.Context,
                    InternalNamespace = item.InternalNamespace,
                    Value = item.String,
                    Id = item.StringId,
                    Type = Enum.Parse<StringType>(item.Type),
                    SoftwareComment = item.SoftwareDeveloperComment,
                    MasterTranslatorComment = item.MasterTranslatorComment
                };
            });

            return await Task.FromResult(result);
        }
    }
}
