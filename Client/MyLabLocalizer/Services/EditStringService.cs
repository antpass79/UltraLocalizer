using MyLabLocalizer.Models;
using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Core.Extensions;
using MyLabLocalizer.Core.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    class EditStringService : IEditStringService
    {
        private const string ENDPOINT_READ = "read/";
        private const string ENDPOINT_WRITE = "write/";

        private const string ENDPOINT_StringView = "StringView";
        private const string ENDPOINT_Context = "Context";
        private const string ENDPOINT_StringType = "StringType";
        private const string ENDPOINT_Concept = "Concept";
        private const string ENDPOINT_String = "String";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public EditStringService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddress());
        }

        async public Task<IEnumerable<StringView>> GetStringViewsAsync(StringViewSearch search)
        {
            var result = await _secureHttpClient.SendAsync(HttpMethod.Get, ENDPOINT_READ + ENDPOINT_StringView, search);
            return await result.GetValue<IEnumerable<StringView>>();
        }

        async public Task<IEnumerable<Context>> GetContextsAsync()
        {
            return await _secureHttpClient.GetAsync<IEnumerable<Context>>(ENDPOINT_READ + ENDPOINT_Context);
        }

        async public Task<IEnumerable<StringType>> GetStringTypesAsync()
        {
            return await _secureHttpClient.GetAsync<IEnumerable<StringType>>(ENDPOINT_READ + ENDPOINT_StringType);
        }

        async public Task SaveAsync(SavableConceptModel savableConceptModel)
        {
            await _secureHttpClient.PutAsync(ENDPOINT_WRITE + ENDPOINT_Concept, savableConceptModel);
        }

        async public Task UpdateAsync(TranslatedString translatedString)
        {
            await _secureHttpClient.PutAsync(ENDPOINT_WRITE + ENDPOINT_String, translatedString);
        }

    }
}
