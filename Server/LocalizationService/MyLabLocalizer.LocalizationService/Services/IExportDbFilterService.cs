using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IExportDbFilterService
    {
        IEnumerable<Language> GetLanguages(ExportDbFilters exportDbFilters);
        IEnumerable<ComponentNamespaceGroup<ComponentNamespace, InternalNamespace>> GetComponentNamespaceGroups(ExportDbFilters exportDbFilters);
    }
}
