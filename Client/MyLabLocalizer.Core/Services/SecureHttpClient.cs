using MyLabLocalizer.Core.Extensions;
using Globe.Client.Platofrm.Events;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyLabLocalizer.Core.Services
{
    public class SecureHttpClient : IAsyncSecureHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IGlobeDataStorage _globeDataStorage;

        public SecureHttpClient(HttpClient httpClient, IGlobeDataStorage globeDataStorage)
        {
            _httpClient = httpClient;
            _globeDataStorage = globeDataStorage;
        }

        public void BaseAddress(string baseAddress)
        {
            _httpClient.BaseAddress = new Uri(baseAddress);
        }

        async public Task<HttpResponseMessage> SendAsync<T>(HttpMethod method, string requestUri, T data)
        {
            var tokenInfo = await _globeDataStorage.GetAsync();
            if (tokenInfo != null && !string.IsNullOrWhiteSpace(tokenInfo.Token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenInfo.Token);
            else
                _httpClient.DefaultRequestHeaders.Authorization = null;

            var json = JsonConvert.SerializeObject(data);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _httpClient.SendAsync(new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(_httpClient.BaseAddress + requestUri),
                Content = stringContent
            });
            if (!result.IsSuccessStatusCode)
                throw new Exception($"The request uri is {requestUri}");

            return result;
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
                throw new Exception($"The request uri is {requestUri}");

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
                throw new Exception($"The request uri is {requestUri}");

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
                throw new Exception($"The request uri is {requestUri}");

            return result;
        }
    }
}