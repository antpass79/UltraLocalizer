﻿using System.Net.Http;
using System.Threading.Tasks;

namespace MyLabLocalizer.Core.Services
{
    public interface IAsyncSecureHttpClient
    {
        void BaseAddress(string baseAddress);
        Task<T> GetAsync<T>(string requestUri);
        Task<HttpResponseMessage> SendAsync<T>(HttpMethod method, string requestUri, T data);
        Task<HttpResponseMessage> PostAsync<T>(string requestUri, T data);
        Task<HttpResponseMessage> PutAsync<T>(string requestUri, T data);
    }
}