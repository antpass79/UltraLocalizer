using MyLabLocalizer.Models;
using MyLabLocalizer.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    public interface IHttpLocalizableStringService : IAsyncLocalizableStringService
    {
    }

    public class HttpLocalizableStringService : IHttpLocalizableStringService
    {
        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public HttpLocalizableStringService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddress());
        }

        async public Task<IEnumerable<LocalizableString>> GetAllAsync()
        {
            return await _secureHttpClient.GetAsync<IEnumerable<LocalizableString>>("LocalizableString");
        }

        async public Task SaveAsync(IEnumerable<LocalizableString> strings)
        {
            await _secureHttpClient.PutAsync("LocalizableString", strings);
        }
    }
}
