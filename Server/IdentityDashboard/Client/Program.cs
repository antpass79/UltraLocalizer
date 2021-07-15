using Blazored.LocalStorage;
using MyLabLocalizer.IdentityDashboard.Client.Components;
using MyLabLocalizer.IdentityDashboard.Client.Handlers;
using MyLabLocalizer.IdentityDashboard.Client.Providers;
using MyLabLocalizer.IdentityDashboard.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<TableSortService, TableSortService>();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IGlobeDataStorage, GlobeLocalStorage>();
            builder.Services.AddScoped<IApplicationService, ApplicationService>();

            //builder.Services.AddTransient(services => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<SpinnerService>();
            builder.Services.AddScoped<AutoSpinnerHttpMessageHandler>();
            builder.Services.AddScoped(services =>
            {
                var accessTokenHandler = services.GetRequiredService<AutoSpinnerHttpMessageHandler>();
                accessTokenHandler.InnerHandler = new HttpClientHandler();
                var uriHelper = services.GetRequiredService<NavigationManager>();
                return new HttpClient(accessTokenHandler)
                {
                    BaseAddress = new Uri(uriHelper.BaseUri)
                };
            });
            await builder.Build().RunAsync();

            // https://stackoverflow.com/questions/60793142/decoding-jwt-in-blazore-client-side-results-wasm-system-argumentexception-idx1
            _ = new JwtPayload();
        }
    }
}
