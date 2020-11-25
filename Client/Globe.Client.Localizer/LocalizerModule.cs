﻿using Globe.Client.Localizer.Dialogs.ViewModels;
using Globe.Client.Localizer.Dialogs.Views;
using Globe.Client.Localizer.Services;
using Globe.Client.Localizer.ViewModels;
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
            regionManager.RegisterViewWithRegion(RegionNames.MAIN_REGION, typeof(JobListManagementWindow));
            regionManager.RegisterViewWithRegion(RegionNames.TOOLBAR_REGION, typeof(JobListManagementWindowToolBar));
            regionManager.RegisterViewWithRegion(RegionNames.MAIN_REGION, typeof(CurrentJobWindow));
            regionManager.RegisterViewWithRegion(RegionNames.TOOLBAR_REGION, typeof(CurrentJobWindowToolBar));
            regionManager.RegisterViewWithRegion(RegionNames.MAIN_REGION, typeof(JobsWindow));
            regionManager.RegisterViewWithRegion(RegionNames.TOOLBAR_REGION, typeof(JobsWindowToolBar));
            regionManager.RegisterViewWithRegion(RegionNames.MAIN_REGION, typeof(MergeWindow));
            regionManager.RegisterViewWithRegion(RegionNames.TOOLBAR_REGION, typeof(MergeWindowToolBar));

            ActivateDefaultView(containerProvider);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ICheckConnectionService, CheckConnectionService>();
            containerRegistry.Register<IAsyncSecureHttpClient, SecureHttpClient>();
            containerRegistry.Register<IProxyLocalizableStringService, ProxyLocalizableStringService>();
            containerRegistry.Register<IFileSystemLocalizableStringService, FileSystemLocalizableStringService>();
            containerRegistry.Register<IHttpLocalizableStringService, HttpLocalizableStringService>();
            containerRegistry.Register<IHttpLocalizableStringService, HttpLocalizableStringService>();
            containerRegistry.Register<ICurrentJobFiltersService, CurrentJobFiltersService>();
            containerRegistry.Register<ICurrentJobConceptViewService, CurrentJobConceptViewService>();
            containerRegistry.Register<IEditStringService, EditStringService>();
            containerRegistry.Register<IStringMergeService, StringsMergeService>();
            containerRegistry.Register<IJobListManagementFiltersService, JobListManagementFiltersService>();
            containerRegistry.Register<IJobListManagementService, JobListManagementService>();
            containerRegistry.RegisterSingleton<IPreviewStyleService, RemotePreviewStyleService>();

            containerRegistry.RegisterDialog<StringEditorDialog, StringEditorDialogViewModel>();
            containerRegistry.RegisterDialog<SaveJoblistDialog, SaveJoblistViewModel>();
            containerRegistry.RegisterDialog<MessageDialog, MessageDialogViewModel>();
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
