using Globe.TranslationServer.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncGroupedStringEntityService : IAsyncReadService<ConceptViewDTO>
    {
        public Task<IEnumerable<ConceptViewDTO>> GetAllAsync(string componentNamespace, string internalNamespace, int languageId, int jobListId);
    }

    public interface IAsyncXmlGroupedStringEntityService : IAsyncReadService<ConceptViewDTO>
    {
        public Task<IEnumerable<ConceptViewDTO>> GetAllAsync(string componentNamespace, string internalNamespace, int languageId, int jobListId);
    }
}
