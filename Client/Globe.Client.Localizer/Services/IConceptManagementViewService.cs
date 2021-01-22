using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IConceptManagementViewService
    {
        Task<IEnumerable<TranslatedConcept>> GetTranslatedConceptSAsync(ConceptManagementSearch search);
    }
}
