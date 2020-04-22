namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models
{
    public class MultiLangString
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string LocalizationID { get; set; }
        public string ContextName { get; set; }
        public string StringType { get; set; }
        public string[] MLString { get; set; }
    }
}