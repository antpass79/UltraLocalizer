using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public interface IAsyncSecureHttpClient
    {
        void BaseAddress(string baseAddress);
        Task<T> GetAsync<T>(string requestUri);
        Task<HttpResponseMessage> PostAsync<T>(string requestUri, T data);
        Task<HttpResponseMessage> PutAsync<T>(string requestUri, T data);
    }
}