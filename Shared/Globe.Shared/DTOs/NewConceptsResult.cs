namespace Globe.Shared.DTOs
{
    public class NewConceptsResult
    {
        public int UpdatedCount { get; set; }
        public int InsertedCount { get; set; }
        public bool ChangesFound => UpdatedCount > 0 || InsertedCount > 0;
        public bool IsNotified { get; set; }
    }
}
