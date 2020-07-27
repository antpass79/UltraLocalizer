using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Repositories
{
    public interface IAsyncUserRepository : IAsyncRepository<ApplicationUser, string>
    {
        Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user);
    }
}
