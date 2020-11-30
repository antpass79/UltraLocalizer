using System.IO;

namespace Globe.Identity.AdministrativeDashboard.Server.Services
{
    public interface IAsyncApplicationService
    {
        Stream Zip();
    }
}
