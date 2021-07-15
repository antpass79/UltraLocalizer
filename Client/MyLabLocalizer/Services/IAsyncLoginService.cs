using MyLabLocalizer.Models;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    internal interface IAsyncLoginService
    {
        Task<LoginResult> LoginAsync(Credentials credentials);
        Task LogoutAsync(Credentials credentials);
    }
}
