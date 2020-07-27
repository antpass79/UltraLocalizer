using Globe.Identity.Models;
using System.Threading.Tasks;

namespace Globe.Identity.Services
{
    public interface IAsyncLoginService
    {
        Task<LoginResult> LoginAsync(Credentials credentials);
        Task LogoutAsync();
    }
}
