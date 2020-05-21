using Globe.Client.Localizer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface ICurrentJobConceptViewsService
    {
        Task<IEnumerable<ConceptView>> GetConceptViewsAsync(ConceptViewSearch search);
    }
}
