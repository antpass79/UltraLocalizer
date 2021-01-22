namespace Globe.Shared.DTOs
{
    public class TranslatedConceptDetail
    {
        public string ContextName { get; set; }
        public int Concept2ContextId { get; set; }
        public StringType StringType { get; set; }
        public string LocalizedString { get; set; }
        public string Language { get; set; }
    }
}
