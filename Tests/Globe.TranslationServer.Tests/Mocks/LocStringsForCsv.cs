using CsvHelper.Configuration.Attributes;

namespace MyLabLocalizer.LocalizationService.Tests.Mocks
{
    public class LocStringsForCsv
    {
        [Index(0)]
        public int ID { get; set; }
        [Index(1)]
        public int IDLanguage { get; set; }
        [Index(2)]
        public int IDType { get; set; }
        [Index(3)]
        public string String { get; set; }
    }
}