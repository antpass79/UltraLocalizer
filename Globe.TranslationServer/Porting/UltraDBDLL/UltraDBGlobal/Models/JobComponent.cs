using System.Collections.Generic;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models
{
    public class JobComponent
    {
        public string ComponentNamespace { get; set; }
        public List<JobInternal> InternalName { get; set; }
    }
}