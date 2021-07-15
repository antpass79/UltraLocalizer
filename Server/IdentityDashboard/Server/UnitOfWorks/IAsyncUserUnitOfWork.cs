using Globe.BusinessLogic;
using MyLabLocalizer.IdentityDashboard.Server.Repositories;

namespace MyLabLocalizer.IdentityDashboard.Server.UnitOfWorks
{
    public interface IAsyncUserUnitOfWork : IAsyncSaveable
    {
        IAsyncUserRepository UserRepository { get; }
    }
}
