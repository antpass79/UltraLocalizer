namespace Globe.TranslationServer.Porting.UltraDBDLL.DataTables
{
    public class DataTableEditConcept
    {
        public int ID { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string LocalizationID { get; set; }
        public string ContextName { get; set; }
        public bool Ignore { get; set; }
        public int IDConcept { get; set; }
        public int IDContext { get; set; }
        public string Comment { get; set; }
    }
}
