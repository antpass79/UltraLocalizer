﻿using MyLabLocalizer.Core.Services;
using MyLabLocalizer.Core.Utilities;
using Globe.Client.Platofrm.Events;
using Prism.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Core.ViewModels
{
    public abstract class LocalizeWindowViewModel : AuthorizeWindowViewModel
    {
        protected LocalizeWindowViewModel(
            IIdentityStore identityStore,
            IEventAggregator eventAggregator,
            ILocalizationAppService localizationAppService)
            : base(identityStore, eventAggregator)
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

        IDictionary<string, string> _localize = new LocalizedDictionary();
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
