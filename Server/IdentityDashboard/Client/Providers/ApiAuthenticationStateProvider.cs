using Globe.Identity.AdministrativeDashboard.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IGlobeDataStorage _localStorage;

        public ApiAuthenticationStateProvider(HttpClient httpClient, IGlobeDataStorage localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var data = await _localStorage.GetAsync();
            if (string.IsNullOrWhiteSpace(data.Token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", data.Token);

            return new AuthenticationState(ClaimsPrincipalGenerator.BuildClaimsPrincipal(data.Token, data.UserName));
        }

        async public Task MarkUserAsAuthenticated(string userName)
        {
            var data = await _localStorage.GetAsync();

            var authenticatedUser = ClaimsPrincipalGenerator.BuildClaimsPrincipal(data.Token, userName);
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
