using System.Collections.Generic;

namespace MyLabLocalizer.Shared.DTOs
{
    public class TranslatedConcept
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Concept { get; set; }
        public IList<TranslatedConceptDetail> TranslatedConceptDetails { get; set; }
    }
}
