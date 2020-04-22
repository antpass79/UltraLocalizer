namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models
{
    public class DBConcept
    {
        public int IDConcept { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string LocalizationID { get; set; }
        public bool Ignore { get; set; }
        public string Comment { get; set; }
        public bool Mark2Delete { get; set; }
        public int IDConcept2Context { get; set; }
        public string Context { get; set; }
    }
}
