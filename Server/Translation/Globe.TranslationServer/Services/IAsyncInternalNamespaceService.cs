using Globe.TranslationServer.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncInternalNamespaceService : IAsyncReadService<InternalNamespaceDTO>
    {
        Task<IEnumerable<InternalNamespaceDTO>> GetAllAsync(string componentNamespace);
    }
}
