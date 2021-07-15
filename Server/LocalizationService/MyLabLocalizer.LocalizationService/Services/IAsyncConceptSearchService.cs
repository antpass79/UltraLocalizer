using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.DTOs;
using MyLabLocalizer.LocalizationService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
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
