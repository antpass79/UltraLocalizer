using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Identity;
using Globe.Client.Platform.Services;
using Globe.Client.Platofrm.Events;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Configuration;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class HttpLoginService : IAsyncLoginService
    {
        private readonly HttpClient _httpClient;
        private readonly IEventAggregator _eventAggregator;
        IGlobeDataStorage _globeDataStorage;

        public HttpLoginService(HttpClient httpClient, IEventAggregator eventAggregator, IGlobeDataStorage globeDataStorage)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["LoginBaseAddress"]);
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
