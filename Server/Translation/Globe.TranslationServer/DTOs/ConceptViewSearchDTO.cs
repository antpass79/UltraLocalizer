namespace Globe.TranslationServer.DTOs
{
    public enum WorkingMode
    {
        FromXml = 0,
        FromDatabase = 1,
        EditUnsatisfiedConstrains = 2
    }

    public class ConceptViewSearchDTO
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string ISOCoding { get; set; }
        public int JobListId { get; set; }
        public WorkingMode WorkingMode { get; set; }
    }
}
