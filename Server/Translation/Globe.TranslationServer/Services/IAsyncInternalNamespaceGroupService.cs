using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncInternalNamespaceGroupService
    {
        Task<IEnumerable<InternalNamespaceGroup>> GetAllAsync(Language language);
    }
}