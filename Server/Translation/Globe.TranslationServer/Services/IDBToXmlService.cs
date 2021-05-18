using Globe.Shared.DTOs;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IDBToXmlService
    {
        Task Generate(string outputFolder, ExportDbFilters exportDbFilters, bool debugMode = true);
    }
}
