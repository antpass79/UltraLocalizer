using Globe.Identity.AdministrativeDashboard.Client.Services;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Pages
{
    public class RegisterUserDataModel : ComponentBase
    {
        [Inject]
        public IAuthService authService { get; set; }
        [Inject]
        public NavigationManager UrlNavigationManager { get; set; }

        protected RegistrationDTO registration = new RegistrationDTO();

        protected bool ShowErrors;
        protected IEnumerable<string> Errors;

        protected async Task HandleRegistration()
        {
            ShowErrors = false;

            var result = await authService.Register(registration);
            if (result.Successful)
            {
                UrlNavigationManager.NavigateTo("user/login");
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