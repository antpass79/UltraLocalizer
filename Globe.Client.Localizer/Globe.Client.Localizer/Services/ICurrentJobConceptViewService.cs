using Globe.Client.Localizer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface ICurrentJobConceptViewService
    {
        Task<IEnumerable<ConceptView>> GetConceptViewsAsync(ConceptViewSearch search);
        Task<ConceptDetails> GetConceptDetailsAsync(ConceptView concept);
    }
}
