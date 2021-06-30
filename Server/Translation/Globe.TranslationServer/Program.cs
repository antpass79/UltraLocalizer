using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Authentication;

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
                    var defaultWebBuilder = webBuilder
                    .ConfigureLogging((context, configureLogging) =>
                    {
                        configureLogging.AddConfiguration(context.Configuration.GetSection("Logging"));

                        configureLogging.ClearProviders();
                        configureLogging.AddConsole();
                        configureLogging.AddDebug();
                        configureLogging.AddAzureWebAppDiagnostics();
                    })
                    .ConfigureServices(services =>
                    {
                        services
                            .Configure<AzureFileLoggerOptions>(options =>
                            {
                                options.FileName = "azure-diagnostics-";
                                options.FileSizeLimit = 50 * 1024;
                                options.RetainedFileCountLimit = 5;
                            })
                            .Configure<AzureBlobLoggerOptions>(options =>
                            {
                                options.BlobName = "log.txt";
                            });
                    })
                        .UseStartup<Startup>();

                    var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    if (string.Compare(environmentName, "Development", true) == 0)
                    {
                        defaultWebBuilder
                            .UseKestrel((context, serverOptions) =>
                            {
                                serverOptions.Configure(context.Configuration.GetSection("Kestrel"))
                                .Endpoint("HTTPS", listenOptions =>
                                {
                                    listenOptions.HttpsOptions.SslProtocols = SslProtocols.Tls12;
                                });
                            });
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<FileWatcherService>();
                });
    }
}
