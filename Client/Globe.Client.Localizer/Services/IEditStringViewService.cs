using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IEditStringViewService
    {
        Task<IEnumerable<LocalizeString>> GetTranslatedConceptsAsync(EditStringSearch search);
    }
}
