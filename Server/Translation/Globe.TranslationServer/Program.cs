using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace Globe.TranslationServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, options) =>
                {
                    Console.WriteLine($"Environment Name {context.HostingEnvironment.EnvironmentName}");

                    options
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                        //.AddUserSecrets("bf78c344-a2b0-4f2f-aa96-41bb0828bc8b");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                    .UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 5001, listenOptions =>
                        {
                            var folder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Certificate");
                            listenOptions.UseHttps(Path.Combine(folder, "identity.pfx"), "identity");
                        });
                    });
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<FileWatcherService>();
                });
    }
}
