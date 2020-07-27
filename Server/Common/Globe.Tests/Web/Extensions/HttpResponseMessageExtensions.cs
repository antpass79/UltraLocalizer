using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Tests.Web.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        async public static Task<T> GetValue<T>(this HttpResponseMessage httpResponseMessage)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<T>(content);

            return value;
        }
    }
}
