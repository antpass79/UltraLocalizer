using Globe.TranslationServer.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncNotTranslatedConceptViewService
    {
        Task<IEnumerable<NotTranslatedConceptViewDTO>> GetAllAsync(NotTranslateConceptViewSearchDTO search);
    }
}
