using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    interface IEditStringViewService
    {
        Task<IEnumerable<LocalizeString>> GetTranslatedConceptsAsync(EditStringSearch search);
    }
}
