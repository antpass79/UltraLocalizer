using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Identity;
using Globe.Client.Platform.Services;
using Globe.Client.Platofrm.Events;
using Prism.Events;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class InMemoryLoginService : IAsyncLoginService
    {
        Dictionary<string, string> _credentials = new Dictionary<string, string>();
        private readonly IEventAggregator _eventAggregator;
        IGlobeDataStorage _globeDataStorage;

        public InMemoryLoginService(IEventAggregator eventAggregator, IGlobeDataStorage globeDataStorage)
        {
            _eventAggregator = eventAggregator;
            _globeDataStorage = globeDataStorage;

            _credentials.Add("admin", "admin");
            _credentials.Add("guest", "guest");
            _credentials.Add("usermanager", "usermanager");
        }

        async public Task<LoginResult> LoginAsync(Credentials credentials)
        {
            if (string.IsNullOrWhiteSpace(credentials.UserName) || string.IsNullOrWhiteSpace(credentials.Password) || !_credentials.ContainsKey(credentials.UserName))
            {
                await _globeDataStorage.RemoveAsync();
                OnPrincipalChanged(new AnonymousPrincipal());

                return await Task.FromResult(new LoginResult
                {
                    Successful = false,
                    Error = "Invalid Credentials",
                    Token = string.Empty
                });
            }

            var result = _credentials[credentials.UserName] == credentials.Password;
            if (!result)
            {
                await _globeDataStorage.RemoveAsync();
                OnPrincipalChanged(new AnonymousPrincipal());

                return await Task.FromResult(new LoginResult
                {
                    Successful = false,
                    Error = "Invalid Credentials",
                    Token = string.Empty
                });

            }

            await _globeDataStorage.StoreAsync(new GlobeLocalStorageData
            {
                Token = "Valid Token",
                UserName = credentials.UserName
            });
            OnPrincipalChanged(ClaimsPrincipalGenerator.BuildClaimsPrincipal("Valid Token", credentials.UserName));

            return await Task.FromResult(new LoginResult
            {
                Successful = true,
                Error = string.Empty,
                Token = "Valid Token"
            });
        }

        async public Task LogoutAsync(Credentials credentials)
        {
            await _globeDataStorage.RemoveAsync();
            OnPrincipalChanged(new AnonymousPrincipal());
        }

        private void OnPrincipalChanged(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            _eventAggregator.GetEvent<PrincipalChangedEvent>().Publish(principal);
        }
    }
}
