using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Services;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    public interface IHttpLocalizableStringService : IAsyncLocalizableStringService
    {
    }

    public class HttpLocalizableStringService : IHttpLocalizableStringService
    {
        IAsyncSecureHttpClient _secureHttpClient;

        public HttpLocalizableStringService(IAsyncSecureHttpClient secureHttpClient)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(ConfigurationManager.AppSettings["LocalizableStringBaseAddress"]);
        }

        async public Task<IEnumerable<LocalizableString>> GetAllAsync()
        {
            return await _secureHttpClient.GetAsync<IEnumerable<LocalizableString>>("LocalizableString");
        }

        async public Task SaveAsync(IEnumerable<LocalizableString> strings)
        {
            await _secureHttpClient.PutAsync<IEnumerable<LocalizableString>>("LocalizableString", strings);
        }
    }
}
