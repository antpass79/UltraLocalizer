using Globe.BusinessLogic;
using Globe.BusinessLogic.Repositories;
using MyLabLocalizer.IdentityDashboard.Server.Models;
using MyLabLocalizer.IdentityDashboard.Server.Repositories;

namespace MyLabLocalizer.IdentityDashboard.Server.UnitOfWorks
{
    public interface IAsyncRoleUnitOfWork : IAsyncSaveable
    {
        IAsyncRoleRepository RoleRepository { get; }
    }
}
