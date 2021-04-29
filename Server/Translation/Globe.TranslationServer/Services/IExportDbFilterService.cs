using Globe.Shared.DTOs;
using System.Collections.Generic;

namespace Globe.TranslationServer.Services
{
    public interface IExportDbFilterService
    {
        IEnumerable<Language> GetLanguages(ExportDbFilters exportDbFilters);
        IEnumerable<ComponentNamespaceGroup<ComponentNamespace, InternalNamespace>> GetComponentNamespaceGroups(ExportDbFilters exportDbFilters);
    }
}
