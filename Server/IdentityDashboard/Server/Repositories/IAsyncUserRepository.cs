using Globe.BusinessLogic.Repositories;
using MyLabLocalizer.IdentityDashboard.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Server.Repositories
{
    public interface IAsyncUserRepository : IAsyncRepository<ApplicationUser, string>
    {
        Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user);
    }
}
