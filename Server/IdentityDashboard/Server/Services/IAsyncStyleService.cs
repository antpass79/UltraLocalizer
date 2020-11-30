using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Services
{
    public interface IAsyncStyleService
    {
        Task<string> GetFileContent(string filePath);
    }
}
