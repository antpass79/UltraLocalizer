using Globe.Client.Localizer.Services;
using Globe.Client.Localizer.Views;
using Globe.Client.Platform;
using Globe.Client.Platform.Services;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Globe.Client.Localizer
{
    class LocalizerModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.MAIN_REGION, typeof(HomeWindow));
            regionManager.RegisterViewWithRegion(RegionNames.TOOLBAR_REGION, typeof(HomeWindowToolBar));
            regionManager.RegisterViewWithRegion(RegionNames.MAIN_REGION, typeof(LoginWindow));
            regionManager.RegisterViewWithRegion(RegionNames.MAIN_REGION, typeof(JobsWindow));
            regionManager.RegisterViewWithRegion(RegionNames.TOOLBAR_REGION, typeof(JobsWindowToolBar));
            regionManager.RegisterViewWithRegion(RegionNames.MAIN_REGION, typeof(MergeWindow));
            regionManager.RegisterViewWithRegion(RegionNames.TOOLBAR_REGION, typeof(MergeWindowToolBar));

            ActivateDefaultView(containerProvider);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILocalizationService, FakeLocalizationService>();
            containerRegistry.Register<ICheckConnectionService, CheckConnectionService>();
            containerRegistry.Register<IAsyncSecureHttpClient, SecureHttpClient>();
            containerRegistry.Register<IProxyLocalizableStringService, ProxyLocalizableStringService>();
            containerRegistry.Register<IFileSystemLocalizableStringService, FileSystemLocalizableStringService>();
            containerRegistry.Register<IHttpLocalizableStringService, HttpLocalizableStringService>();
            containerRegistry.Register<IStringMergeService, StringsMergeService>();
        }

        private void ActivateDefaultView(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            IRegion region = regionManager.Regions[RegionNames.MAIN_REGION];
            var view = containerProvider.Resolve<HomeWindow>();
            
            region.Add(view);
            region.Activate(view);
        }
    }
}
