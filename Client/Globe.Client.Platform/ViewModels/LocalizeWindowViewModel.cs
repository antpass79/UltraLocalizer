using Globe.Client.Platform.Services;
using Globe.Client.Platofrm.Events;
using Prism.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Platform.ViewModels
{
    public abstract class LocalizeWindowViewModel : AuthorizeWindowViewModel
    {
        protected LocalizeWindowViewModel(IEventAggregator eventAggregator, ILocalizationAppService localizationAppService)
            : base(eventAggregator)
        {
            LocalizationAppService = localizationAppService;

            EventAggregator.GetEvent<LanguageChangedEvent>()
                .Subscribe(async (language) =>
                {
                    await OnLanguageChanged(language);
                });

            ChangeLanguage(LocalizationAppService.SelectedLanguage);
        }

        protected ILocalizationAppService LocalizationAppService { get; }

        IDictionary<string, string> _localize = new Dictionary<string, string>();
        public IDictionary<string, string> Localize
        {
            get => _localize;
            private set
            {
                SetProperty<IDictionary<string, string>>(ref _localize, value);
            }
        }

        protected void ChangeLanguage(string language)
        {
            EventAggregator
                .GetEvent<LanguageChangedEvent>()
                .Publish(language);
        }

        async virtual protected Task OnLanguageChanged(string language)
        {
            this.Localize = await LocalizationAppService.LoadAsync(language);
        }
    }
}
