using Globe.Identity.AdministrativeDashboard.Server.Repositories;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.UnitOfWorks
{
    public class RoleUnitOfWork : IAsyncRoleUnitOfWork
    {
        public IAsyncRoleRepository RoleRepository { get; }

        public RoleUnitOfWork(IAsyncRoleRepository roleRepository)
        {
            RoleRepository = roleRepository;
        }

        async public Task SaveAsync()
        {
            await Task.CompletedTask;
        }
    }
}
