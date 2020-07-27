using System.ComponentModel.DataAnnotations;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models
{
    public class DBConceptSearch
    {
        [Key]
        public int progressiveID { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string LocalizationID { get; set; }
        public bool Ignore { get; set; }
        public string MTComment { get; set; }
        public string SWComment { get; set; }
        public int IDString { get; set; }
        public string String { get; set; }
        public string Context { get; set; }
        public string Type { get; set; }
    }
}
