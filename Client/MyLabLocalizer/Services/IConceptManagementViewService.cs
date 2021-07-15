using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    interface IConceptManagementViewService
    {
        Task<IEnumerable<TranslatedConcept>> GetTranslatedConceptSAsync(ConceptManagementSearch search);
    }
}
