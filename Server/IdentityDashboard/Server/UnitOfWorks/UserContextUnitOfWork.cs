using Globe.Identity.AdministrativeDashboard.Server.Data;
using Globe.Identity.AdministrativeDashboard.Server.Repositories;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.UnitOfWorks
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
