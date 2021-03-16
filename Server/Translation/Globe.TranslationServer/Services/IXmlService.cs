using Globe.Shared.DTOs;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IXmlService
    {
        Task<XmlServiceStatistics> UpdateDatabaseAsync();
        string GetOriginalDeveloperString(string componentNamespace, string internalNamespace, string conceptId, string context);
        string GetSoftwareDeveloperComment(string componentNamespace, string internalNamespace, string conceptId);
    }
}
