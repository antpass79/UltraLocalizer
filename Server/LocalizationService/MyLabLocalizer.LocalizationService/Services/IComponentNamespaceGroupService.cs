using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IComponentNamespaceGroupService
    {
        Task<IEnumerable<ComponentNamespaceGroup>> GetAllAsync(Language language);
        Task<IEnumerable<ComponentNamespaceGroup>> GetAllAsync();
    }
}