using Globe.TranslationServer.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncConceptViewItemProxyService
    {
        Task<IEnumerable<ConceptViewItemDTO>> GetAllAsync(ConceptViewItemSearchDTO search);
    }
}
