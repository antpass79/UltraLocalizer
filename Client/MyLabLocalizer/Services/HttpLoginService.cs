using MyLabLocalizer.Models;
using Globe.Client.Platofrm.Events;
using MyLabLocalizer.Core.Extensions;
using MyLabLocalizer.Core.Identity;
using MyLabLocalizer.Core.Services;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    class HttpLoginService : IAsyncLoginService
    {
        private readonly HttpClient _httpClient;
        private readonly IEventAggregator _eventAggregator;
        private readonly IGlobeDataStorage _globeDataStorage;

        public HttpLoginService(
            HttpClient httpClient, 
            IEventAggregator eventAggregator, 
            IGlobeDataStorage globeDataStorage, 
            ISettingsService settingsService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settingsService.GetIdentitytBaseAddress());
            _eventAggregator = eventAggregator;
            _globeDataStorage = globeDataStorage;
        }

        async public Task<LoginResult> LoginAsync(Credentials credentials)
        {
            try
            {
                var loginResult = await OnLoginAsync(credentials);
                if (loginResult.Successful)
                {
                    await _globeDataStorage.StoreAsync(new GlobeLocalStorageData
                    {
                        Token = loginResult.Token,
                        UserName = credentials.UserName
                    });
                    await OnPrincipalChanged(ClaimsPrincipalGenerator.BuildClaimsPrincipal(loginResult.Token, credentials.UserName));
                }
                else
                {
                    await _globeDataStorage.RemoveAsync();
                    await OnPrincipalChanged(new AnonymousPrincipal());
                }

                return loginResult;
            }
            catch (Exception e)
            {
                await _globeDataStorage.RemoveAsync();
                await OnPrincipalChanged(new AnonymousPrincipal());

                return new LoginResult
                {
                    Successful = false,
                    Error = e.Message,
                    Token = string.Empty
                };
            }
        }

        async public Task LogoutAsync(Credentials credentials)
        {
            await _globeDataStorage.RemoveAsync();
            await OnPrincipalChanged(new AnonymousPrincipal());
        }

        async private Task<LoginResult> OnLoginAsync(Credentials credentials)
        {
            var json = JsonConvert.SerializeObject(credentials);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponseMessage = await _httpClient.PostAsync("Login", stringContent);
            var loginResult = await httpResponseMessage.GetValue<LoginResult>();

            return loginResult != null ? loginResult : new LoginResult
            {
                Successful = false,
                Error = $"Error from Server: {httpResponseMessage.StatusCode.ToString()}",
                Token = string.Empty
            };
        }

        async private Task OnPrincipalChanged(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            _eventAggregator.GetEvent<PrincipalChangedEvent>().Publish(principal);
            await Task.CompletedTask;
        }
    }
}
