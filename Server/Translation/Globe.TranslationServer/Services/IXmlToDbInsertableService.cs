using Globe.TranslationServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IXmlToDbInsertableService
    {
        Task<IEnumerable<ConceptTupla>> InsertFilteredEntriesIntoDatabaseAsync(Dictionary<string, LocalizationSectionSubset> keyLocalizationSectionSubsetPairs);
    }
}
