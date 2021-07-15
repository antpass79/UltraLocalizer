using MyLabLocalizer.IdentityDashboard.Server.Data;
using MyLabLocalizer.IdentityDashboard.Server.Repositories;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Server.UnitOfWorks
{
    public class UserContextUnitOfWork : IAsyncUserUnitOfWork
    {
        public IAsyncUserRepository UserRepository { get; }
        readonly ApplicationDbContext _context;

        public UserContextUnitOfWork(IAsyncUserRepository userRepository, ApplicationDbContext context)
        {
            UserRepository = userRepository;
            _context = context;
        }

        async public Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
