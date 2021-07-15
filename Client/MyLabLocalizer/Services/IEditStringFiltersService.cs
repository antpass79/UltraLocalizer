using MyLabLocalizer.Models;
using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    interface IEditStringFiltersService
    {
        Task<IEnumerable<Language>> GetLanguagesAsync();
    }
}
