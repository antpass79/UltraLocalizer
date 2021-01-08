namespace Globe.Shared.DTOs
{
    public class ConceptManagementSearch
    {     
        public int LanguageId { get; set; }
        public string Context { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Concept { get; set; }
        public string LocalizedString { get; set; }
    }
}
