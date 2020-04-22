namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models
{
    public class JobInternal
    {
        private static int IDcounter = 0;
        public string ComponentNamespace { get; set; }
        public int ID { get; set; }
        public string InternalNamespace { get; set; }
        public JobInternal()
        {
            ID = IDcounter++;
        }
    }
}