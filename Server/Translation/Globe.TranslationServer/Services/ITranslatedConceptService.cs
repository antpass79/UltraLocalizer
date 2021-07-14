using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface ITranslatedConceptService : IAsyncReadService<TranslatedConcept>
    {
        public Task<IEnumerable<TranslatedConcept>> GetAllAsync(string componentNamespace, string internalNamespace, int languageId, string context, string concept, string localizedString, int stringId);

        public Task<IEnumerable<LocalizeString>> GetAllStringsAsync(string componentNamespace, string internalNamespace, int languageId, string context, string concept, string localizedString, int stringId);

    }

}
