using Globe.Client.Localizer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetUsersAsync(Language language);
    }
}
