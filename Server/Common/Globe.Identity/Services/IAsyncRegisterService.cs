using Globe.Identity.Models;
using System.Threading.Tasks;

namespace Globe.Identity.Servicess
{
    public interface IAsyncRegisterService
    {
        Task<RegistrationResult> RegisterAsync(Registration registration);
        Task<RegistrationResult> ChangePasswordAsync(RegistrationNewPassword registration);
    }
}
