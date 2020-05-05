using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncInternalConceptsService : IAsyncReadService<InternalConceptsTable>
    {
        Task<IEnumerable<InternalConceptsTable>> GetAllAsync(string componentNamespace);
    }
}
