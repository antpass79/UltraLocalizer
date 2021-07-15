using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IAsyncNotTranslatedConceptViewService
    {
        Task<IEnumerable<NotTranslatedConceptView>> GetAllAsync(NotTranslatedConceptViewSearch search);
    }
}
