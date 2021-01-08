using Globe.Client.Localizer.Models;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IConceptManagementViewService
    {
        Task<IEnumerable<ConceptTranslated>> GetConceptViewsAsync(ConceptManagementSearch search);
    }
}
