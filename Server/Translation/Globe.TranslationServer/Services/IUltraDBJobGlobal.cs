using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using System.Collections.Generic;

namespace Globe.TranslationServer.Services
{
    public interface IUltraDBJobGlobal
    {
        List<JobComponent> GetMissingDataBy(string isocoding);
        List<JobGroupedStringEntity> FillByComponentNamespace(string InternalNamespace, string ComponentName, string isocoding);
        List<JobGroupedStringEntity> FillByComponentNamespaceEng(string InternalNamespace, string ComponentName);
   }
}
