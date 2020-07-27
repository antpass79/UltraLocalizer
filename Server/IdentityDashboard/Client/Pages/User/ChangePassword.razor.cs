using Globe.Identity.AdministrativeDashboard.Client.Services;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Pages
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