using System.Collections.Generic;

namespace MyLabLocalizer.Shared.DTOs
{
    public class ExportDbFilters
    {
        public IEnumerable<Language> Languages { get; set; }
        public IEnumerable<ComponentNamespaceGroup<ComponentNamespace, InternalNamespace>> ComponentNamespaceGroups { get; set; }
    }
}
