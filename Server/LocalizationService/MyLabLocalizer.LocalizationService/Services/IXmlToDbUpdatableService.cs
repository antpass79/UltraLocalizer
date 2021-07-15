using MyLabLocalizer.LocalizationService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IXmlToDbUpdatableService
    {
        Task<IEnumerable<ConceptTupla>> UpdateFilteredEntriesIntoDatabaseAsync(Dictionary<string, LocalizationSectionSubset> keyLocalizationSectionSubsetPairs);
    }
}
