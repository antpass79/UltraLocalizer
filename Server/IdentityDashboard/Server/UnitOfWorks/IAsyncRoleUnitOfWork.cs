using Globe.BusinessLogic;
using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.AdministrativeDashboard.Server.Repositories;

namespace Globe.Identity.AdministrativeDashboard.Server.UnitOfWorks
{
    public interface IAsyncRoleUnitOfWork : IAsyncSaveable
    {
        IAsyncRoleRepository RoleRepository { get; }
    }
}
