using Globe.Identity.AdministrativeDashboard.Client.Components;
using Globe.Identity.AdministrativeDashboard.Client.Services;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Pages
{
    public class RolesDataModel : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; }
        [Inject]
        public NavigationManager UrlNavigationManager { get; set; }
        protected string SearchString { get; set; }
        [Inject]
        protected TableSortService TableSortService { get; set; }

        protected ApplicationRoleDTO[] roles;
        protected ApplicationRoleDTO selectedRole;
        protected bool ShowConfirmation { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            this.TableSortService.Sorted = (IEnumerable<object> items) =>
            {
                this.roles = items.Cast<ApplicationRoleDTO>().ToArray();
            };

            await GetRoles();
        }

        async protected Task EditRole(ApplicationRoleDTO role)
        {
            await Http.SendJsonAsync(HttpMethod.Put, "/api/Role", role);
        }

        async protected Task OnDialogButtonClick(ButtonType buttonType)
        {
            if (buttonType == ButtonType.Yes)
            {
                await DeleteRole(selectedRole);
            }

            ShowConfirmation = false;
            await Task.CompletedTask;
        }

        protected void DeleteRoleConfirm(ApplicationRoleDTO role)
        {
            this.selectedRole = roles.FirstOrDefault(item => item.Id == role.Id);
            this.ShowConfirmation = true;
        }

        async protected Task DeleteRole(ApplicationRoleDTO role)
        {
            await Http.DeleteAsync($"api/Role/{role.Id}");
            await GetRoles();
        }

        async protected Task SearchRoles()
        {
            await GetRoles();
            if (!string.IsNullOrEmpty(SearchString))
            {
                roles = roles.Where(role => role.Name.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1).ToArray();
            }
        }

        async private Task GetRoles()
        {
            try
            {
                roles = await Http.GetJsonAsync<ApplicationRoleDTO[]>("api/Role");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); // inject a logger

                UrlNavigationManager.NavigateTo("/unauthorized");
            }
        }
    }
}