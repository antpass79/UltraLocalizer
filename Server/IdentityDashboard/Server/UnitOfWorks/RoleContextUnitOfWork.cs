using MyLabLocalizer.IdentityDashboard.Server.Data;
using MyLabLocalizer.IdentityDashboard.Server.Repositories;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Server.UnitOfWorks
{
    public class RoleContextUnitOfWork : IAsyncRoleUnitOfWork
    {
        public IAsyncRoleRepository RoleRepository { get; }
        readonly ApplicationDbContext _context;

        public RoleContextUnitOfWork(IAsyncRoleRepository roleRepository, ApplicationDbContext context)
        {
            RoleRepository = roleRepository;
            _context = context;
        }

        async public Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
