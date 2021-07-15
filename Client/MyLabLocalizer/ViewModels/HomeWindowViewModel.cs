using MyLabLocalizer.Core.Services;
using MyLabLocalizer.Core.ViewModels;
using Prism.Events;

namespace MyLabLocalizer.ViewModels
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
