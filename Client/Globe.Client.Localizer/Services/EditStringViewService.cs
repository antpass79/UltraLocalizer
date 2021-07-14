﻿using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class EditStringViewService : IEditStringViewService
    {
        private const string ENDPOINT_EditStringView = "EditStringView";

        private readonly IAsyncSecureHttpClient _secureHttpClient;

        public EditStringViewService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddressRead());
        }

        async public Task<IEnumerable<LocalizeString>> GetTranslatedConceptsAsync(EditStringSearch search)
        {
            var result = await _secureHttpClient.SendAsync(HttpMethod.Get, ENDPOINT_EditStringView, search);
            return await result.GetValue<IEnumerable<LocalizeString>>();
        }
    }
}
