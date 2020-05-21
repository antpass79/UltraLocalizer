using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class EditStringService : IEditStringService
    {
        private const string ENDPOINT_StringView = "StringView";
        private const string ENDPOINT_Context = "Context";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public EditStringService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddress());
        }

        async public Task<IEnumerable<StringView>> GetStringViewsAsync(StringViewSearch search)
        {
            var result = await _secureHttpClient.SendAsync<StringViewSearch>(HttpMethod.Get, ENDPOINT_StringView, search);
            return await result.GetValue<IEnumerable<StringView>>();
        }

        async public Task<IEnumerable<Context>> GetContextsAsync()
        {
            return await _secureHttpClient.GetAsync<IEnumerable<Context>>(ENDPOINT_Context);
        }
    }
}
