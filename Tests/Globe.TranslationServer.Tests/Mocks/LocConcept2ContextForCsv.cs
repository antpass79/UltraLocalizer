using CsvHelper.Configuration.Attributes;

namespace MyLabLocalizer.LocalizationService.Tests.Mocks
{
    internal class LocConcept2ContextForCsv
    {
        [Index(0)]
        public int ID { get; set; }
        [Index(1)]
        public int IDConcept { get; set; }
        [Index(2)]
        public int IDContext { get; set; }
    }
}