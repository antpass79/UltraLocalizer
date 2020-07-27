using Globe.Identity.AdministrativeDashboard.Client.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Pages
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