using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IComponentNamespaceGroupService
    {
        Task<IEnumerable<ComponentNamespaceGroup>> GetAllAsync(Language language);
        Task<IEnumerable<ComponentNamespaceGroup>> GetAllAsync();
    }
}