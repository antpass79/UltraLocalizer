using Globe.Client.Platform.Extensions;
using Globe.Client.Platofrm.Events;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public class SecureHttpClient : IAsyncSecureHttpClient
    {
        private readonly HttpClient _httpClient;
        IEventAggregator _eventAggregator;
        private readonly IGlobeDataStorage _globeDataStorage;

        public SecureHttpClient(HttpClient httpClient, IEventAggregator eventAggregator, IGlobeDataStorage globeDataStorage)
        {
            _httpClient = httpClient;
            _eventAggregator = eventAggregator;
            _globeDataStorage = globeDataStorage;
        }

        public void BaseAddress(string baseAddress)
        {
            _httpClient.BaseAddress = new System.Uri(baseAddress);
        }

        async public Task<T> GetAsync<T>(string requestUri)
        {
            var tokenInfo = await _globeDataStorage.GetAsync();
            if (tokenInfo != null && !string.IsNullOrWhiteSpace(tokenInfo.Token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenInfo.Token);
            else
                _httpClient.DefaultRequestHeaders.Authorization = null;

            var httpResponseMessage = await _httpClient.GetAsync(requestUri);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new Exception();

            return await httpResponseMessage.GetValue<T>();
        }

        async public Task<HttpResponseMessage> PostAsync<T>(string requestUri, T data)
        {
            var tokenInfo = await _globeDataStorage.GetAsync();
            if (tokenInfo != null && !string.IsNullOrWhiteSpace(tokenInfo.Token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenInfo.Token);
            else
                _httpClient.DefaultRequestHeaders.Authorization = null;

            var json = JsonConvert.SerializeObject(data);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync(requestUri, stringContent);
            if (!result.IsSuccessStatusCode)
                throw new Exception();

            return result;
        }

        async public Task<HttpResponseMessage> PutAsync<T>(string requestUri, T data)
        {
            var tokenInfo = await _globeDataStorage.GetAsync();
            if (tokenInfo != null && !string.IsNullOrWhiteSpace(tokenInfo.Token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenInfo.Token);
            else
                _httpClient.DefaultRequestHeaders.Authorization = null;

            var json = JsonConvert.SerializeObject(data);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _httpClient.PutAsync(requestUri, stringContent);
            if (!result.IsSuccessStatusCode)
                throw new Exception();

            return result;
        }
    }
}