using System.Net.Http;
using System.Threading.Tasks;
using Globe.Client.Platform.Extensions;

namespace Globe.Client.Platform.Services
{
    public class StyleService : IStyleService
    {
        #region Data Members

        private const string ENDPOINT_Style = "Style";
        private readonly IAsyncSecureHttpClient _secureHttpClient;

        #endregion

        #region Constructors

        public StyleService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetIdentitytBaseAddress());
        }

        public async Task<string> Get(string stylePath)
        {
            var response = await _secureHttpClient.SendAsync<object>(HttpMethod.Get, $"{ENDPOINT_Style}/?filePath={stylePath}", null);
            return await response.GetValue();
        }

        #endregion
    }
}
