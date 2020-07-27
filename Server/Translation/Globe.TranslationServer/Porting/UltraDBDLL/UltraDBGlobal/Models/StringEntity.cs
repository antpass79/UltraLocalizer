namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models
{
    public class StringEntity
    {
        public int IDContext { get; set; }
        public int IDConcept { get; set; }
        public int IDConcept2Context { get; set; }
        public int IDContext2String { get; set; }
        public int IDString { get; set; }
        public bool Ignore { get; set; }
        public int StringTypeID { get; set; }
        public string StringType { get; set; }
        public string ContextName { get; set; }
        public string DataString { get; set; }
        public string StringENG { get; set; }
        public int OldIDString { get; set; }
        public string LocalizationID { get; set; }
        public string UserString { get; set; }
    }
}