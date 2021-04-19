using Globe.Client.Platform.Models;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.ViewModels;
using Prism.Commands;
using Prism.Events;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace Globe.Client.Localizer.ViewModels
{
    internal class HomeWindowViewModel : LocalizeWindowViewModel
    {
        public HomeWindowViewModel(
            IIdentityStore identityStore,
            IEventAggregator eventAggregator,
            ILocalizationAppService localizationAppService,
            ISettingsService settingsService)
            : base(identityStore, eventAggregator, localizationAppService)
        {
            DownloadStyleUri = new Uri($"{settingsService.GetIdentitytBaseAddress()}style");
            DownloadApplicationUri = new Uri(settingsService.GetApplicationDownloadAddress());
        }

        public Uri DownloadStyleUri { get; }
        public Uri DownloadApplicationUri { get; }

        private DelegateCommand _downloadStyleCommand = null;
        public DelegateCommand DownloadStyleCommand =>
            _downloadStyleCommand ?? (_downloadStyleCommand = new DelegateCommand(() =>
            {
            }));

        private DelegateCommand _downloadApplicationCommand = null;
        public DelegateCommand DownloadApplicationCommand =>
            _downloadApplicationCommand ?? (_downloadApplicationCommand = new DelegateCommand(() =>
            {
                Process.Start(new ProcessStartInfo(DownloadApplicationUri.ToString()) { UseShellExecute = true, WindowStyle = ProcessWindowStyle.Maximized });
            }));
    }
}
