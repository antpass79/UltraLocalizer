using Microsoft.JSInterop;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Services
{
    public class ApplicationService : IApplicationService
    {
        const string APPLICATION_ENDPOINT = "api/application";
        const string SCRIPT_NAME = "SaveFileAs";

        private readonly IJSRuntime _jsRuntime;

        public ApplicationService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        private readonly HttpClient _httpClient;

        async public Task Download()
        {
            var message = await _httpClient.GetAsync(
                APPLICATION_ENDPOINT);

            await Download(message.Content);
        }

        async private Task Download(HttpContent content)
        {
            await _jsRuntime.InvokeAsync<object>(
                SCRIPT_NAME,
                "ultraLocalizer.zip",
                content);
        }
    }
}
