using Globe.TranslationServer.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncInternalNamespaceGroupService
    {
        Task<IEnumerable<InternalNamespaceGroupDTO>> GetAllAsync(LanguageDTO language);
    }
}