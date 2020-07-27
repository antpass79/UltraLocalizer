using Blazored.LocalStorage;
using Globe.Identity.AdministrativeDashboard.Client.Components;
using Globe.Identity.AdministrativeDashboard.Client.Handlers;
using Globe.Identity.AdministrativeDashboard.Client.Providers;
using Globe.Identity.AdministrativeDashboard.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<TableSortService, TableSortService>();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IGlobeDataStorage, GlobeLocalStorage>();
            builder.Services.AddScoped<SpinnerService>();
            builder.Services.AddScoped<AutoSpinnerHttpMessageHandler>();
            builder.Services.AddScoped(serviceProvider =>
            {
                var blazorDisplaySpinnerAutomaticallyHttpMessageHandler = serviceProvider.GetRequiredService<AutoSpinnerHttpMessageHandler>();
                var wasmHttpMessageHandlerType = Assembly.Load("WebAssembly.Net.Http").GetType("WebAssembly.Net.Http.HttpClient.WasmHttpMessageHandler");
                var wasmHttpMessageHandler = (HttpMessageHandler)Activator.CreateInstance(wasmHttpMessageHandlerType);

                blazorDisplaySpinnerAutomaticallyHttpMessageHandler.InnerHandler = wasmHttpMessageHandler;
                var uriHelper = serviceProvider.GetRequiredService<NavigationManager>();
                Console.WriteLine("return new HttpClient");
                return new HttpClient(blazorDisplaySpinnerAutomaticallyHttpMessageHandler)
                {
                    BaseAddress = new Uri(uriHelper.BaseUri)
                };
            });

            builder.RootComponents.Add<App>("app");

            builder.Services.AddBaseAddressHttpClient();

            var host = builder.Build();
            // https://stackoverflow.com/questions/60793142/decoding-jwt-in-blazore-client-side-results-wasm-system-argumentexception-idx1
            _ = new System.IdentityModel.Tokens.Jwt.JwtPayload();
            await host.RunAsync();
        }
    }
}
