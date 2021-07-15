using MyLabLocalizer.Shared.DTOs;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IXmlToDBService
    {
        Task<XmlServiceStatistics> UpdateDatabaseAsync();
        string GetOriginalDeveloperString(string componentNamespace, string internalNamespace, string conceptId, string context);
        string GetSoftwareDeveloperComment(string componentNamespace, string internalNamespace, string conceptId);
    }
}
