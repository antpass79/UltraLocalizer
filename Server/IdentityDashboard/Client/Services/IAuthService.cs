using MyLabLocalizer.IdentityDashboard.Shared.DTOs;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Client.Services
{
    public interface IAuthService
    {
        Task<RegistrationResultDTO> Register(RegistrationDTO registration);
        Task<RegistrationResultDTO> ChangePassword(RegistrationNewPasswordDTO registration);
        Task<LoginResultDTO> Login(CredentialsDTO credentials);
        Task Logout();
    }
}