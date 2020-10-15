using Globe.Client.Platform.Services;
using Globe.Client.Platform.ViewModels;
using Prism.Events;

namespace Globe.Client.Localizer.ViewModels
{
    internal class HomeWindowViewModel : LocalizeWindowViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public HomeWindowViewModel(IEventAggregator eventAggregator, ILocalizationAppService localizationAppService)
            : base(eventAggregator, localizationAppService)
        {
        }
    }
}
