namespace Globe.TranslationServer.DTOs
{
    public enum ConceptSearchBy
    {
        Concept = 0,
        String = 1
    }

    public enum ConceptFilterBy
    {
        None = 0,
        Context = 1,
        StringType = 2
    }

    public class StringViewSearchDTO
    {
        public string StringValue { get; set; }
        public string Context { get; set; }
        public StringTypeDTO StringType { get; set; }
        public ConceptSearchBy SearchBy { get; set; }
        public ConceptFilterBy FilterBy { get; set; }
        public string ISOCoding { get; set; }
    }
}
