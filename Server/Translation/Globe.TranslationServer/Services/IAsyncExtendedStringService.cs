using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncExtendedStringService : IAsyncReadService<DBExtendedStrings>
    {
        Task<IEnumerable<DBExtendedStrings>> GetAllAsync(string componentName, string internalNamespace, string ISOCoding, int jobListId, int conceptId);
    }
}
