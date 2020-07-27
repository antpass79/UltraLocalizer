using System.Collections.Generic;

namespace Globe.Client.Localizer.Models
{
    class ConceptView
    {
        public int Id { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Name { get; set; }
        public IEnumerable<ContextView> ContextViews { get; set; }

        public string DetailsLink { get; set; }
    }
}
