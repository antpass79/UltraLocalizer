using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncConceptSearchService
    {
        Task<IEnumerable<ConceptSearch>> GetSearchConceptbyISO(string ISO, string search, bool allData);
        Task<IEnumerable<ConceptSearch>> GetSearchConceptbyISObyContext(string ISO, string search, string context, bool allData);
        Task<IEnumerable<ConceptSearch>> GetSearchConceptbyISObyStringType(string ISO, string search, string stringType, bool allData);
        Task<IEnumerable<ConceptSearch>> GetSearchStringbyISO(string ISO, string search, bool allData);
        Task<IEnumerable<ConceptSearch>> GetSearchStringtbyISObyContext(string ISO, string search, string context, bool allData);
        Task<IEnumerable<ConceptSearch>> GetSearchStringtbyISObyStringType(string ISO, string search, string stringType, bool allData);
    }
}
