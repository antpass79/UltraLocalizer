using System.Net.Http;
using System.Threading.Tasks;
using MyLabLocalizer.Core.Extensions;

namespace MyLabLocalizer.Core.Services
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

        #endregion

        #region Public Functions

        public async Task<string> Get(string stylePath)
        {
            var response = await _secureHttpClient.SendAsync<object>(HttpMethod.Get, $"{ENDPOINT_Style}/?filePath={stylePath}", null);
            return await response.GetValue();
        }

        #endregion
    }
}
