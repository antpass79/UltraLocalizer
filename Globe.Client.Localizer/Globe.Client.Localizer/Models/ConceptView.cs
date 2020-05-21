using System.Collections.Generic;

namespace Globe.Client.Localizer.Models
{
    class ConceptView
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Name { get; set; }
        public IEnumerable<ContextView> ContextViews { get; set; }
    }
}
