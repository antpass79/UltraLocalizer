using Globe.Client.Localizer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IStringEditingService
    {
        Task<IEnumerable<ConceptViewItem>> GetConceptViewItemsAsync(ConceptViewItemSearch search);
    }
}
