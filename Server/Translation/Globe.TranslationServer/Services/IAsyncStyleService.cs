using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncStyleService
    {
        Task<string> GetFileContent(string filePath);
    }
}
