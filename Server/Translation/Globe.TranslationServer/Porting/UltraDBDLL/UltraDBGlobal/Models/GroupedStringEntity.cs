using System.Collections.Generic;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models
{
    public class GroupedStringEntity
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string LocalizationID { get; set; }
        public bool Ignore { get; set; }
        public int ConceptID { get; set; }
        public List<StringEntity> Group { get; set; }
    }
}