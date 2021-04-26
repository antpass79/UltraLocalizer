using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class ExportDbFiltersService : IExportDbFiltersService
    {
        private const string ENDPOINT_READ = "read/";

        private const string ENDPOINT_ComponentNamespaceGroup = "TranslatedComponentNamespaceGroup";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public ExportDbFiltersService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddress());
        }

        async public Task<IEnumerable<BindableComponentNamespaceGroup>> GetAllComponentNamespaceGroupsAsync()
        {
            return await _secureHttpClient.GetAsync<IEnumerable<BindableComponentNamespaceGroup>>(ENDPOINT_READ + ENDPOINT_ComponentNamespaceGroup);
        }
    }
}
