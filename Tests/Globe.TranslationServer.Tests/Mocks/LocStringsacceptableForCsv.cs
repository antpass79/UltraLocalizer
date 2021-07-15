using CsvHelper.Configuration.Attributes;

namespace MyLabLocalizer.LocalizationService.Tests.Mocks
{
    internal class LocStringsacceptableForCsv
    {
        [Index(0)]
        public int ID { get; set; }
        [Index(1)]
        public int ID_String { get; set; }
    }
}