using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IConceptTranslatedService : IAsyncReadService<ConceptTranslated>
    {
        public Task<IEnumerable<ConceptTranslated>> GetAllAsync(string componentNamespace, string internalNamespace, int languageId, string context, string concept, string localizedString);
    }

}
