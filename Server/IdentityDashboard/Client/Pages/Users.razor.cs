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
    public class UsersDataModel : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; }
        [Inject]
        public NavigationManager UrlNavigationManager { get; set; }
        [Inject]
        protected TableSortService TableSortService { get; set; }

        protected string SearchString { get; set; }

        protected ApplicationUserDTO[] users;
        protected ApplicationUserDTO selectedUser;
        protected bool ShowConfirmation { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            this.TableSortService.Sorted = (IEnumerable<object> items) =>
            {
                this.users = items.Cast<ApplicationUserDTO>().ToArray();
            };

            await GetUsers();
        }

        async protected Task EditUser(ApplicationUserDTO user)
        {
            await Http.SendJsonAsync(HttpMethod.Put, "/api/User", user);
        }

        async protected Task OnDialogButtonClick(ButtonType buttonType)
        {
            if (buttonType == ButtonType.Yes)
            {
                await DeleteUser(selectedUser);
            }

            ShowConfirmation = false;
            await Task.CompletedTask;
        }

        protected void DeleteUserConfirm(ApplicationUserDTO user)
        {
            this.selectedUser = users.FirstOrDefault(item => item.Id == user.Id);
            this.ShowConfirmation = true;
        }

        async protected Task DeleteUser(ApplicationUserDTO user)
        {
            await Http.DeleteAsync($"api/User/{user.Id}");
            await GetUsers();
        }

        async protected Task SearchUsers()
        {
            await GetUsers();
            if (!string.IsNullOrEmpty(SearchString))
            {
                users = users.Where(user => user.FullName.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1).ToArray();
            }
        }

        async private Task GetUsers()
        {
            try
            {
                users = await Http.GetJsonAsync<ApplicationUserDTO[]>("api/User");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); // inject a logger

                UrlNavigationManager.NavigateTo("/unauthorized");
            }
        }
    }
}