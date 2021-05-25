using Globe.Client.Platform.Services;
using Globe.Client.Platform.ViewModels;
using Prism.Events;

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

        }
    }
}
