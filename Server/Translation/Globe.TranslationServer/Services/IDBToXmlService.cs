using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IDBToXmlService
    {
        Task Generate(string outputFolder, bool debugMode = true);
    }
}
