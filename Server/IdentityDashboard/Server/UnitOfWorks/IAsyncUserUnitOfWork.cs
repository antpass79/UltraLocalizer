using Globe.BusinessLogic;
using Globe.Identity.AdministrativeDashboard.Server.Repositories;

namespace Globe.Identity.AdministrativeDashboard.Server.UnitOfWorks
{
    public interface IAsyncUserUnitOfWork : IAsyncSaveable
    {
        IAsyncUserRepository UserRepository { get; }
    }
}
