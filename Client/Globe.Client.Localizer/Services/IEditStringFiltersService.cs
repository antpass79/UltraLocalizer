using Globe.Client.Localizer.Models;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IEditStringFiltersService
    {
        Task<IEnumerable<Language>> GetLanguagesAsync();
    }
}
