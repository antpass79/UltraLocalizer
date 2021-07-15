using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    class EditStringFiltersService : IEditStringFiltersService
    {
        private const string ENDPOINT_Language = "Language";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public EditStringFiltersService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddressRead());
        }

        async public Task<IEnumerable<Language>> GetLanguagesAsync()
        {
            return await _secureHttpClient.GetAsync<IEnumerable<Language>>(ENDPOINT_Language);
        }
    }
}
