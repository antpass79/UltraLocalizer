using MyLabLocalizer.IdentityDashboard.Client.Services;
using MyLabLocalizer.IdentityDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Client.Pages
{
    public class ChangePasswordDataModel : ComponentBase
    {
        [Inject]
        public IAuthService authService { get; set; }
        [Inject]
        public NavigationManager UrlNavigationManager { get; set; }

        protected RegistrationNewPasswordDTO registration = new RegistrationNewPasswordDTO();

        protected bool ShowErrors;
        protected IEnumerable<string> Errors;

        protected async Task HandleRegistration()
        {
            ShowErrors = false;

            var result = await authService.ChangePassword(registration);
            if (result.Successful)
            {
                UrlNavigationManager.NavigateTo("/");
            }
            else
            {
                Errors = result.Errors;
                ShowErrors = true;
            }
        }

        protected void Cancel()
        {
            UrlNavigationManager.NavigateTo("/");
        }
    }
}