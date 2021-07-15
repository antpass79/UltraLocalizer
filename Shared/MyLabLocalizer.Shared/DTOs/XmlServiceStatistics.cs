namespace MyLabLocalizer.Shared.DTOs
{
    public class XmlServiceStatistics
    {
        public int UpdatedCount { get; set; }
        public int InsertedCount { get; set; }
        public bool ChangesFound => UpdatedCount > 0 || InsertedCount > 0;
    }
}
