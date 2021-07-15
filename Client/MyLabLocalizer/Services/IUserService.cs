using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetUsersAsync(Language language);

        Task<IEnumerable<ApplicationUser>> GetUsersAsync(string userName);
    }
}
