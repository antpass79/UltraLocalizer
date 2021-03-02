using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IXmlService
    {
        bool ChangesFound { get; }
        int UpdatedCount { get; }
        int InsertedCount { get; }

        Task<int> UpdateDatabaseAsync();
        void LoadXml();
        void FillDB();
        string GetOriginalDeveloperString(string componentNamespace, string internalNamespace, string conceptId, string context);
        string GetSoftwareDeveloperComment(string componentNamespace, string internalNamespace, string conceptId);
    }
}
