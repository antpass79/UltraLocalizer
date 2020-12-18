namespace Globe.Shared.DTOs
{
    public class JobListContext
    {
        public string Name { get; set; }
        public int Concept2ContextId { get; set; }

        public StringType StringType { get; set; }
        public string StringValue { get; set; }
        public int StringId { get; set; }
        public int OldStringId { get; set; }
    }
}
