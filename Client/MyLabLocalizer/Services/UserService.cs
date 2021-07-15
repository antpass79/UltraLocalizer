using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Core.Extensions;
using MyLabLocalizer.Core.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    class UserService : IUserService
    {
        private const string ENDPOINT_User = "User/GetUserByLanguage";
        private const string ENDPOINT_UserByUserPermission = "User/GetUserByPermission";

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

        async public Task<IEnumerable<ApplicationUser>> GetUsersAsync(string userName)
        {
            var result = await _secureHttpClient.SendAsync<string>(HttpMethod.Get, ENDPOINT_UserByUserPermission, userName);
            return await result.GetValue<IEnumerable<ApplicationUser>>();
        }
    }
}
