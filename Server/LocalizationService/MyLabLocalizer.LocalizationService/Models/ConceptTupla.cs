using System.Collections.Generic;

namespace MyLabLocalizer.LocalizationService.Models
{
    public class ConceptTupla
    {
        public string IdConcept2Context { get; set; }
        public string Id { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string ConceptId { get; set; }
        public string ContextId { get; set; }
        public string Comments { get; set; }
        public IEnumerable<string> Strings { get; set; }
        public string FileName { get; set; }
        public ConceptTuplaActionType ActionType { get; set; }
    }
}
