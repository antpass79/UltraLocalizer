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
        private const string ENDPOINT_StringViewItem = "StringViewItem";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public CurrentJobStringItemsService(IAsyncSecureHttpClient secureHttpClient)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(ConfigurationManager.AppSettings["LocalizableStringBaseAddress"]);
        }

        async public Task<IEnumerable<StringViewItem>> GetStringViewItemsAsync(StringViewItemSearch search)
        {
            var result = await _secureHttpClient.SendAsync<StringViewItemSearch>(HttpMethod.Get, ENDPOINT_StringViewItem, search);
            return await result.GetValue<IEnumerable<StringViewItem>>();
        }
    }
}
