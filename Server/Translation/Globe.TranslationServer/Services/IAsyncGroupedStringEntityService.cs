using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncGroupedStringEntityService : IAsyncReadService<GroupedStringEntity>
    {
        public Task<IEnumerable<GroupedStringEntity>> GetAllAsync(string componentNamespace, string InternalNamespace, string ISOCoding, int jobListId);
    }
}
