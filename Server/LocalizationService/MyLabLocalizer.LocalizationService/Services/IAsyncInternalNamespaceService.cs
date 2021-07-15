using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IAsyncInternalNamespaceService : IAsyncReadService<InternalNamespace>
    {
        Task<IEnumerable<InternalNamespace>> GetAllAsync(string componentNamespace);
    }
}
