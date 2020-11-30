using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class UserService : IUserService
    {
        private const string ENDPOINT_User = "User/GetUserByLanguage";
        
        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public UserService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetIdentitytBaseAddress());
        }

        async public Task<IEnumerable<ApplicationUser>> GetUsersAsync(Language language)
        {
            var result = await _secureHttpClient.SendAsync<Language>(HttpMethod.Get, ENDPOINT_User, language);
            return await result.GetValue<IEnumerable<ApplicationUser>>();
        }
    }
}
