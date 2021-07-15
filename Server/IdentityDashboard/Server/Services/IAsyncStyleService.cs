using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Server.Services
{
    public interface IAsyncStyleService
    {
        Task<string> GetFileContent(string filePath);
    }
}
