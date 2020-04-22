using Globe.Client.Localizer.Models;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    internal interface IAsyncLoginService
    {
        Task<LoginResult> LoginAsync(Credentials credentials);
        Task LogoutAsync(Credentials credentials);
    }
}
