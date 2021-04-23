using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class ExportDbFiltersService : IExportDbFiltersService
    {
        private const string ENDPOINT_READ = "read/";

        private const string ENDPOINT_ComponentNamespaceGroup = "ComponentNamespaceGroup";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public ExportDbFiltersService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddress());
        }

        async public Task<IEnumerable<BindableComponentNamespaceGroup>> GetAllComponentNamespaceGroupsAsync()
        {
            var result = await _secureHttpClient.SendAsync<object>(HttpMethod.Get, ENDPOINT_READ + ENDPOINT_ComponentNamespaceGroup, null);
            return await result.GetValue<IEnumerable<BindableComponentNamespaceGroup>>();
        }
    }
}
