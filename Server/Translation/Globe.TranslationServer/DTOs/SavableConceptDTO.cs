namespace Globe.TranslationServer.DTOs
{
    public class SavableConceptDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }  
        public LanguageDTO Language { get; set; }       
        public ComponentNamespaceDTO ComponentNamespace { get; set; }
        public InternalNamespaceDTO InternalNamespace { get; set; }
    }
}
