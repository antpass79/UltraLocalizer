namespace Globe.Client.Localizer.Models
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

    public class StringViewSearch
    {
        public string StringValue { get; internal set; }
        public string Context { get; internal set; }
        public StringType StringType { get; internal set; }
        public ConceptSearchBy SearchBy { get; internal set; }
        public ConceptFilterBy FilterBy { get; internal set; }
        public string ISOCoding { get; internal set; }
    }
}
