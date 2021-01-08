using System.Collections.Generic;

namespace Globe.Shared.DTOs
{
    public class ConceptTranslated
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Concept { get; set; }
        public IList<ConceptTranslatedGroup> ConceptTranslatedGroups { get; set; }
    }
}
