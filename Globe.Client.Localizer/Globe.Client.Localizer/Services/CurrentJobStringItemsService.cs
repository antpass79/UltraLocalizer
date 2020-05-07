using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class CurrentJobStringItemsService : ICurrentJobStringItemsService
    {
        private const string ENDPOINT_StringItemView = "StringItemView";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public CurrentJobStringItemsService(IAsyncSecureHttpClient secureHttpClient)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(ConfigurationManager.AppSettings["LocalizableStringBaseAddress"]);
        }

        async public Task<IEnumerable<StringItemView>> GetStringItemsAsync(StringItemViewSearch search)
        {
            var result = await _secureHttpClient.SendAsync<StringItemViewSearch>(HttpMethod.Get, ENDPOINT_StringItemView, search);
            return await result.GetValue<IEnumerable<StringItemView>>();
        }
    }
}
