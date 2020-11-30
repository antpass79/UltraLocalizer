using Globe.Identity.AdministrativeDashboard.Server.DTOs;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Services
{
    public interface IAsyncVersionService
    {
        Task<VersionDTO> Get();
    }
}
