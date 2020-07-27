using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Pages
{
    public class AddEditRoleDataModel : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; }
        [Inject]
        public NavigationManager UrlNavigationManager { get; set; }
        [Parameter]
        public string roleId { get; set; }

        protected string Title = "Add";
        public ApplicationRoleDTO role = new ApplicationRoleDTO();

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(roleId))
            {
                Title = "Edit";
                role = await Http.GetJsonAsync<ApplicationRoleDTO>("/api/Role/" + roleId);
            }
        }

        protected async Task SaveRole()
        {
            if (!string.IsNullOrWhiteSpace(role.Id))
            {
                await Http.SendJsonAsync(HttpMethod.Put, "api/Role/", role);
            }
            else
            {
                await Http.SendJsonAsync(HttpMethod.Post, "/api/Role/", role);
            }

            Cancel();
        }

        public void Cancel()
        {
            UrlNavigationManager.NavigateTo("/roles");
        }
    }
}