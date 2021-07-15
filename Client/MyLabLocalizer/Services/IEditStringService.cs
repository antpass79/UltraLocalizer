using MyLabLocalizer.Models;
using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    interface IEditStringService
    {
        Task<IEnumerable<StringView>> GetStringViewsAsync(StringViewSearch search);
        Task<IEnumerable<Context>> GetContextsAsync();
        Task<IEnumerable<StringType>> GetStringTypesAsync();
        Task SaveAsync(SavableConceptModel savableConceptModel);
        Task UpdateAsync(TranslatedString translatedString);
    }
}
