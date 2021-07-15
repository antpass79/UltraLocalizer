using System.IO;

namespace MyLabLocalizer.IdentityDashboard.Server.Services
{
    public interface IAsyncApplicationService
    {
        Stream Zip();
    }
}
