﻿using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public class XmlService : IXmlService
    {
        #region Data Members

        private const string ENDPOINT_Xml = "Xml";
        private readonly IAsyncSecureHttpClient _secureHttpClient;

        #endregion

        #region Constructors

        public XmlService(IAsyncSecureHttpClient secureHttpClient, ISettingsService settingsService)
        {
            _secureHttpClient = secureHttpClient;
            _secureHttpClient.BaseAddress(settingsService.GetLocalizableStringBaseAddressRead());
        }

        public async Task Download(string downloadPath = "")
        {
            downloadPath = !string.IsNullOrWhiteSpace(downloadPath) ? downloadPath : Path.Combine($"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}", "xml.zip");
            var result = await _secureHttpClient.SendAsync<object>(HttpMethod.Get, ENDPOINT_Xml, null);
            var bytes = await result.Content.ReadAsByteArrayAsync();
            if (File.Exists(downloadPath))
                File.Delete(downloadPath);
            await File.WriteAllBytesAsync(downloadPath, bytes);
        }

        #endregion
    }
}