using System.Collections.Generic;

namespace Globe.Client.Localizer.Models
{
    class StringViewItem
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Concept { get; set; }
        public IEnumerable<ContextViewItem> ContextViewItems { get; set; }
    }
}
