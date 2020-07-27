using Globe.Identity.AdministrativeDashboard.Client.Models;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Pages
{
    public class AddEditUserDataModel : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; }
        [Inject]
        public NavigationManager UrlNavigationManager { get; set; }
        [Parameter]
        public string userId { get; set; }

        protected string Title = "Add";
        protected UserWithRoles userWithRoles = new UserWithRoles();
        protected IList<ApplicationEditableRole> editableRoles = new List<ApplicationEditableRole>();

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Title = "Edit";
                userWithRoles = await Http.GetJsonAsync<UserWithRoles>("/api/User/" + userId);
            }

            await GetRoles();
        }

        protected async Task GetRoles()
        {
            var roles = await Http.GetJsonAsync<ApplicationRoleDTO[]>("api/Role");
            editableRoles = roles.Select(role =>
            {
                return new ApplicationEditableRole
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    Selected = userWithRoles.Roles.ToList().Find(item => item.Id == role.Id) != null
                };
            }).ToList();
        }

        protected async Task SaveUser()
        {
            var userWithRolesToSave = new UserWithRoles
            {
                User = this.userWithRoles.User,
                Roles = editableRoles.Where(role => role.Selected)
            };

            if (!string.IsNullOrWhiteSpace(this.userWithRoles.User.Id))
            {
                await Http.SendJsonAsync(HttpMethod.Put, "api/User/", userWithRolesToSave);
            }
            else
            {
                await Http.SendJsonAsync(HttpMethod.Post, "/api/User/", userWithRolesToSave);
            }

            Cancel();
        }

        protected void Cancel()
        {
            UrlNavigationManager.NavigateTo("/users");
        }
    }
}