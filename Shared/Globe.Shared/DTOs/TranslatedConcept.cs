using System.Collections.Generic;

namespace Globe.Shared.DTOs
{
    public class TranslatedConcept
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Concept { get; set; }
        public IList<TranslatedConceptDetail> TranslatedConceptDetails { get; set; }
    }
}
