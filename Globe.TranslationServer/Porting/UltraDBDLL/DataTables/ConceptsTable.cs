namespace Globe.TranslationServer.Porting.UltraDBDLL.DataTables
{
    public class ConceptsTable
    {
        public int ID { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string LocalizationID { get; set; }
        public bool Ignore { get; set; }
        public string Comment { get; set; }
        public byte[] SSMA_TimeStamp { get; set; }
    }
}
