using System.Collections.Generic;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models
{
    public class MergiableConcept
    {
        public int ConceptId { get; set; }
        public string Concept { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }     
        public string Context { get; set; }
        public ConceptTuplaActionType ActionType { get; set; }
    }
}
