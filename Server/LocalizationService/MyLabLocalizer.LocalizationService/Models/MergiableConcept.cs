namespace MyLabLocalizer.LocalizationService.Models
{
    public class MergiableConcept
    {
        public int ConceptId { get; set; }
        public string Concept { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }     
        public string DeveloperComment { get; set; }
        public string Context { get; set; }
        public ConceptTuplaActionType ActionType { get; set; }
    }
}
