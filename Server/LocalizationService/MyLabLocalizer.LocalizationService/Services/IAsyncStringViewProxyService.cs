using MyLabLocalizer.LocalizationService.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IAsyncStringViewProxyService
    {
        Task<IEnumerable<StringViewDTO>> GetAllAsync(StringViewSearchDTO search);
    }
}
