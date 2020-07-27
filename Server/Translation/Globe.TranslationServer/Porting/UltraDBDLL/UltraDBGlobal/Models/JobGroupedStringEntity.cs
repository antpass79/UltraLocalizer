using System.Collections.Generic;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models
{
    public class JobGroupedStringEntity
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string LocalizationID { get; set; }
        public string MTString { get; set; }
        public int ConceptID { get; set; }
        public List<JobStringEntity> Group { get; set; }
    }
}