namespace Globe.TranslationServer.Models
{
    public class ConceptSearch
    {
        public int ProgressiveId { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Concept { get; set; }
        public bool Ignore { get; set; }
        public string MasterTranslatorComment { get; set; }
        public string SoftwareDeveloperComment { get; set; }
        public int StringId { get; set; }
        public string String { get; set; }
        public string Context { get; set; }
        public string Type { get; set; }
    }
}
