using CsvHelper.Configuration.Attributes;

namespace MyLabLocalizer.LocalizationService.Tests.Mocks
{
    internal class LocContextsForCsv
    {
        [Index(0)]
        public int ID { get; set; }
        [Index(1)]
        public string ContextName { get; set; }
    }
}