namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models
{
    public class DBGlobal
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string LocalizationID { get; set; }
        public string ISOCoding { get; set; }
        public string ContextName { get; set; }
        public string DataString { get; set; }
        public int DatabaseID { get; set; }
        public bool IsAcceptable { get; set; }
        public bool IsToIgnore { get; set; }
        public int Concept2ContextID { get; set; }
    }
}