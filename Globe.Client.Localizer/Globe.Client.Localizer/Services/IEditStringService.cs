using Globe.Client.Localizer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IEditStringService
    {
        Task<IEnumerable<StringView>> GetStringViewsAsync(StringViewSearch search);
        Task<IEnumerable<Context>> GetContextsAsync();
        Task SaveAsync();
    }
}
