using MyLabLocalizer.Shared.DTOs;

namespace MyLabLocalizer.LocalizationService.DTOs
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
        public StringType StringType { get; set; }
        public ConceptSearchBy SearchBy { get; set; }
        public ConceptFilterBy FilterBy { get; set; }
        public string ISOCoding { get; set; }
    }
}
