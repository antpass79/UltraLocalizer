using Globe.Identity.AdministrativeDashboard.Client.Providers;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IGlobeDataStorage _localStorage;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           IGlobeDataStorage localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<RegistrationResultDTO> Register(RegistrationDTO registration)
        {
            var result = await _httpClient.PostJsonAsync<RegistrationResultDTO>("api/Register", registration);
            return result;
        }

        public async Task<RegistrationResultDTO> ChangePassword(RegistrationNewPasswordDTO registration)
        {
            var result = await _httpClient.PutJsonAsync<RegistrationResultDTO>("api/Register", registration);
            return result;
        }

        public async Task<LoginResultDTO> Login(CredentialsDTO credentials)
        {
            var loginAsJson = JsonSerializer.Serialize(credentials);
            var response = await _httpClient.PostAsync("api/Login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
            var loginResult = JsonSerializer.Deserialize<LoginResultDTO>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!response.IsSuccessStatusCode || !loginResult.Successful)
            {
                return loginResult;
            }

            await _localStorage.StoreAsync(new GlobeLocalStorageData
            {
                Token = loginResult.Token,
                UserName = credentials.UserName
            });

            await ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(credentials.UserName);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

            return loginResult;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveAsync();

            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
