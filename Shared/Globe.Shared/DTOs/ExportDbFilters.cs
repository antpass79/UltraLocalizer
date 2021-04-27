using System.Collections.Generic;

namespace Globe.Shared.DTOs
{
    public enum ExportDbMode
    {
        Full = 0,
        Custom = 1
    }

    public class ExportDbFilters
    {
        public ExportDbMode ExportDbMode { get; set; }
        public IList<string> IsoCodeLanguages { get; set; }
        public IList<string> ComponentNamespaces { get; set; }
        public IList<string> InternalNamespaces { get; set; }
    }
}
