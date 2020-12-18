using Globe.Client.Localizer.Models;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IEditStringService
    {
        Task<IEnumerable<StringView>> GetStringViewsAsync(StringViewSearch search);
        Task<IEnumerable<Context>> GetContextsAsync();
        Task<IEnumerable<StringType>> GetStringTypesAsync();
        Task SaveAsync(SavableConceptModel savableConceptModel);
    }
}
