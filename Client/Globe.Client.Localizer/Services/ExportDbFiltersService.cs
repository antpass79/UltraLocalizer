using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Services;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class ExportDbFiltersService : IExportDbFiltersService
    {
        private const string ENDPOINT_READ = "read/";

        private const string ENDPOINT_ComponentNamespaceGroup = "TranslatedComponentNamespaceGroup";
        private const string ENDPOINT_Language = "Language";

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

        async public Task<IEnumerable<BindableLanguage>> GetLanguagesAsync()
        {
            var languages = await _secureHttpClient.GetAsync<IEnumerable<Language>>(ENDPOINT_READ + ENDPOINT_Language);
            List<BindableLanguage> bindableLanguages = new List<BindableLanguage>();
            foreach (Language language in languages)
            {
                var bindableLanguage = new BindableLanguage
                {
                    Description = language.Description,
                    Id = language.Id,
                    IsoCoding = language.IsoCoding,
                    Name = language.Name,
                    IsSelected = false
                };

                bindableLanguages.Add(bindableLanguage);
            }

            return bindableLanguages;
        }
    }
}
