using MyLabLocalizer.LocalizationService.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IAsyncLocalizableStringService
    {
        Task<IEnumerable<LocalizableString>> GetAllAsync();
        Task SaveAsync(IEnumerable<LocalizableString> strings);
    }
}
