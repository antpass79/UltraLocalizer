using Globe.BusinessLogic.Repositories;
using MyLabLocalizer.IdentityDashboard.Server.Models;
using MyLabLocalizer.IdentityDashboard.Server.Repositories;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Server.UnitOfWorks
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
