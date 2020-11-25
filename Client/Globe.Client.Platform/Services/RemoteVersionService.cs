using Globe.Client.Platform.Models;
using System;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public class RemoteVersionService : IVersionService
    {
        #region Data Members

        private const string ENDPOINT_Version = "Version";
        private readonly IAsyncSecureHttpClient _secureHttpClient;

        #endregion

        #region Constructors

        public RemoteVersionService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddressRead());
        }

        #endregion

        public async Task<VersionDTO> Get()
        {
            return await _secureHttpClient.GetAsync<VersionDTO>(ENDPOINT_Version);
        }
    }
}
