using MyLabLocalizer.IdentityDashboard.Server.Repositories;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Server.UnitOfWorks
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
