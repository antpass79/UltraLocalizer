using MyLabLocalizer.IdentityDashboard.Client.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Client.Pages
{
    public class LogoutUserDataModel : ComponentBase
    {
        [Inject]
        public IAuthService AuthService { get; set; }
        [Inject]
        public NavigationManager UrlNavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await AuthService.Logout();
            UrlNavigationManager.NavigateTo("/");
        }
    }
}