using Globe.Identity.AdministrativeDashboard.Client.Services;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Pages
{
    public class LoginUserDataModel : ComponentBase
    {
        [Inject]
        public IAuthService AuthService { get; set; }
        [Inject]
        public NavigationManager UrlNavigationManager { get; set; }

        protected CredentialsDTO credentials = new CredentialsDTO();

        protected bool ShowErrors;
        protected string Error = string.Empty;

        protected async Task HandleLogin()
        {
            ShowErrors = false;

            var result = await AuthService.Login(credentials);

            if (result.Successful)
            {
                UrlNavigationManager.NavigateTo("/");
            }
            else
            {
                Error = result.Error;
                ShowErrors = true;
            }
        }

        protected void Cancel()
        {
            UrlNavigationManager.NavigateTo("/");
        }
    }
}