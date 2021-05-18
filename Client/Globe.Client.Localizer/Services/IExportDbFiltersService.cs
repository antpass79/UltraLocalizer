using Globe.Client.Localizer.Models;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IExportDbFiltersService
    {
        Task<IEnumerable<BindableComponentNamespaceGroup>> GetAllComponentNamespaceGroupsAsync();
        Task<IEnumerable<BindableLanguage>> GetLanguagesAsync();
    }
}
