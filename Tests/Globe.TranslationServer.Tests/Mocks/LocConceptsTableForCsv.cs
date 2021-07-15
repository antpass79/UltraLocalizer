using CsvHelper.Configuration.Attributes;
using System;

namespace MyLabLocalizer.LocalizationService.Tests.Mocks
{
    class LocConceptsTableForCsv
    {
        [Index(0)]
        public int ID { get; set; }
        [Index(1)]
        public string ComponentNamespace { get; set; }
        [Index(2)]
        public string InternalNamespace { get; set; }
        [Index(3)]
        public string LocalizationID { get; set; }
        [Index(4)]
        public bool Ignore { get; set; }
        [Index(5)]
        public string Comment { get; set; }
        [Index(6)]
        public byte[] SSMA_TimeStamp { get; set; }
    }
}
