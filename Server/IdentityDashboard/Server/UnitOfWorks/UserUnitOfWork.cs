using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.AdministrativeDashboard.Server.Repositories;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.UnitOfWorks
{
    public class UserUnitOfWork : IAsyncUserUnitOfWork
    {
        public IAsyncUserRepository UserRepository { get; }

        public UserUnitOfWork(IAsyncUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        async public Task SaveAsync()
        {
            await Task.CompletedTask;
        }
    }
}
