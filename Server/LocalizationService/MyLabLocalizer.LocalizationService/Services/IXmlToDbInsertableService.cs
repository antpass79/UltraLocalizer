using MyLabLocalizer.LocalizationService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IXmlToDbInsertableService
    {
        Task<IEnumerable<ConceptTupla>> InsertFilteredEntriesIntoDatabaseAsync(Dictionary<string, LocalizationSectionSubset> keyLocalizationSectionSubsetPairs);
    }
}
