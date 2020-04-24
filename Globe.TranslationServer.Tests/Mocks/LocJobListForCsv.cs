using CsvHelper.Configuration.Attributes;

namespace Globe.TranslationServer.Tests.Mocks
{
    internal class LocJobListForCsv
    {
        [Index(0)]
        public int ID { get; set; }
        [Index(1)]
        public string JobName { get; set; }
        [Index(2)]
        public string UserName { get; set; }
        [Index(3)]
        public int IDIsoCoding { get; set; }
    }
}