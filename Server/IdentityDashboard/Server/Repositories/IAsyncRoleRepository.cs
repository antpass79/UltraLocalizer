using Globe.BusinessLogic.Repositories;
using MyLabLocalizer.IdentityDashboard.Server.Models;

namespace MyLabLocalizer.IdentityDashboard.Server.Repositories
{
    public interface IAsyncRoleRepository : IAsyncRepository<ApplicationRole, string>
    {
    }
}
