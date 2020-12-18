using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncInternalNamespaceService : IAsyncReadService<InternalNamespace>
    {
        Task<IEnumerable<InternalNamespace>> GetAllAsync(string componentNamespace);
    }
}
