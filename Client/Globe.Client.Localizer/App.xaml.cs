using Globe.Client.Localizer.Services;
using Globe.Client.Localizer.Views;
using Globe.Client.Platform.Identity;
using Globe.Client.Platform.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Globe.Client.Localizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.SetThreadPrincipal(new AnonymousPrincipal());

            base.OnStartup(e);
            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44360/NotificationHub")
                .Build();

            //var connection = new HubConnectionBuilder()
            //    .WithUrl("https://localhost:44360/NotificationHub",options => 
            //    {
            //        options.AccessTokenProvider = () => Task.FromResult("MyTest");
            //    })
            //    .Build();

            connection.On<string>("JoblistChanged", data => Console.WriteLine(data));
            connection.On<string>("ConceptsChanged", data => Console.WriteLine(data));

            connection.StartAsync();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IViewNavigationService, ViewNavigationService>();
            containerRegistry.RegisterSingleton<IGlobeDataStorage, GlobeInMemoryStorage>();
            containerRegistry.RegisterSingleton<IAsyncLoginService, HttpLoginService>();
            containerRegistry.RegisterSingleton<ILoggerService, ConsoleLoggerService>();
            containerRegistry.RegisterSingleton<ISettingsService, AppSettingsService>();
            containerRegistry.RegisterSingleton<IUserService, UserService>();
            containerRegistry.RegisterSingleton<ILocalizationAppService, FakeLocalizationAppService>();
            
            containerRegistry.Register<INotificationService, NotificationService>();

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
    }
}
