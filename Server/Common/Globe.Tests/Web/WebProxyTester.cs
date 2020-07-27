using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Globe.Tests.Web
{
    public class WebProxyTester : IDisposable
    {
        TestServer _testServer;
        HttpClient _httpClient;

        public WebProxyTester(TestServer testServer, string baseAddress)
        {
            _testServer = testServer;
            _httpClient = _testServer.CreateClient();
            _httpClient.BaseAddress = new Uri(baseAddress);
        }

        async public Task<HttpResponseMessage> SendAsync<T>(HttpMethod method, string action, T data, string accessToken = null)
        {
            var json = JsonConvert.SerializeObject(data);
            return await SendAsync(method, action, json);
        }

        async public Task<HttpResponseMessage> SendAsync(HttpMethod method, string action, string json, string accessToken = null)
        {
            if (!string.IsNullOrWhiteSpace(accessToken))
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _httpClient.SendAsync(new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(_httpClient.BaseAddress + action),
                Content = stringContent
            });
            _httpClient.DefaultRequestHeaders.Remove("Authorization");

            return result;
        }

        async public Task<T> GetAsync<T>(string action, string accessToken = null)
        {
            var result = await GetAsync(action, accessToken);
            var json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        async public Task<HttpResponseMessage> GetAsync(string action, string accessToken = null)
        {
            if (!string.IsNullOrWhiteSpace(accessToken))
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var result = await _httpClient.GetAsync(action);
            _httpClient.DefaultRequestHeaders.Remove("Authorization");

            return result;
        }

        async public Task<HttpResponseMessage> PostAsync<T>(string action, T data, string accessToken = null)
        {
            var json = JsonConvert.SerializeObject(data);
            return await PostAsync(action, json);
        }

        async public Task<HttpResponseMessage> PostAsync(string action, string json, string accessToken = null)
        {
            if (!string.IsNullOrWhiteSpace(accessToken))
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync(action, stringContent);
            _httpClient.DefaultRequestHeaders.Remove("Authorization");

            return result;
        }

        async public Task<HttpResponseMessage> PutAsync<T>(string action, T data, string accessToken = null)
        {
            var json = JsonConvert.SerializeObject(data);
            return await PutAsync(action, json);
        }

        async public Task<HttpResponseMessage> PutAsync(string action, string json, string accessToken = null)
        {
            if (!string.IsNullOrWhiteSpace(accessToken))
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _httpClient.PutAsync(action, stringContent);
            _httpClient.DefaultRequestHeaders.Remove("Authorization");

            return result;

        }

        #region Dispose Pattern

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _httpClient.Dispose();
                    _testServer.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
