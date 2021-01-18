using Globe.Client.Localizer.Services;
using Globe.Client.Localizer.Views;
using Globe.Client.Platform.Identity;
using Globe.Client.Platform.Services;
using Globe.Shared.Services;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Net.Http;
using System.Windows;
using Unity;

namespace Globe.Client.Localizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += Application_DispatcherUnhandledException;

            AppDomain.CurrentDomain.SetThreadPrincipal(new AnonymousPrincipal());

            base.OnStartup(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IStyleService, StyleService>();
            containerRegistry.Register<IXmlService, XmlService>();
            containerRegistry.Register<IVersionService, RemoteVersionService>("RemoteVersionService");
            containerRegistry.Register<IVersionService, LocalVersionService>("LocalVersionService");
            containerRegistry.Register<IViewNavigationService, ViewNavigationService>();
            containerRegistry.RegisterSingleton<IGlobeDataStorage, GlobeInMemoryStorage>();
            containerRegistry.RegisterSingleton<IAsyncLoginService, HttpLoginService>();
            containerRegistry.RegisterSingleton<ILogService, FileLogService>();
            containerRegistry.RegisterSingleton<ISettingsService, AppSettingsService>();
            containerRegistry.RegisterSingleton<IUserService, UserService>();
            containerRegistry.RegisterSingleton<ILocalizationAppService, FakeLocalizationAppService>();
            containerRegistry.RegisterSingleton<INotificationService, NotificationService>();

            var unityContainer = containerRegistry.GetContainer();
            unityContainer.RegisterFactory<ICompareVersionService>(container =>
            {
                return new CompareVersionService(container.Resolve<IVersionService>("RemoteVersionService"), container.Resolve<IVersionService>("LocalVersionService"));
            },
            FactoryLifetime.Singleton);
            unityContainer.RegisterFactory<HttpClient>(container =>
            {
                var byPassCertificateHandler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                    {
                        Console.WriteLine($"Sender: {sender}");
                        Console.WriteLine($"cert: {cert}");
                        Console.WriteLine($"chain: {chain}");
                        Console.WriteLine($"sslPolicyErrors: {sslPolicyErrors}");
                        return true;
                    }
                };
                return new HttpClient(byPassCertificateHandler);
            },
            FactoryLifetime.Transient);
        }

        protected override Window CreateShell()
        {
            return this.Container.Resolve<MainWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            Type translatorModule = typeof(LocalizerModule);
            moduleCatalog.AddModule(
            new ModuleInfo()
            {
                ModuleName = translatorModule.Name,
                ModuleType = translatorModule.AssemblyQualifiedName,
            });
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var unityContainer = this.Container.GetContainer();
            unityContainer
                .Resolve<ILogService>()
                .Exception(e.Exception);
            unityContainer
                .Resolve<INotificationService>()
                .NotifyAsync("Critical", $"{e.Exception.Message}", Platform.Services.Notifications.NotificationLevel.Error);
        }
    }
}
