using MyLabLocalizer.LocalizationService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IXmlToDbMergeService
    {
        Task<IEnumerable<MergiableConcept>> MergeFilteredEntriesIntoDatabaseAsync(Dictionary<string, LocalizationSectionSubset> keyLocalizationSectionSubsetPairs);
    }
}
