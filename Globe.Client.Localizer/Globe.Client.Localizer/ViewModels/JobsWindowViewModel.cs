using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Globe.Client.Localizer.ViewModels
{
    internal class JobsWindowViewModel : AuthorizeWindowViewModel
    {

        private readonly IAsyncLocalizableStringService _proxyLocalizableStringService;
        private readonly IStringMergeService _stringMergeService;

        public JobsWindowViewModel(IEventAggregator eventAggregator, IProxyLocalizableStringService proxyLocalizableStringService, IStringMergeService stringMergeService)
            : base(eventAggregator)
        {
            _proxyLocalizableStringService = proxyLocalizableStringService;
            _stringMergeService = stringMergeService;
        }

        IEnumerable<LocalizableString> _strings;
        public IEnumerable<LocalizableString> Strings
        {
            get => _strings;
            set
            {
                SetProperty<IEnumerable<LocalizableString>>(ref _strings, value);
            }
        }

        private DelegateCommand _loadCommand = null;
        public DelegateCommand LoadCommand =>
            _loadCommand ?? (_loadCommand = new DelegateCommand(async () =>
            {
                this.Strings = await _proxyLocalizableStringService.GetAllAsync();
                SaveCommand.RaiseCanExecuteChanged();
            }));

        private DelegateCommand _saveCommand = null;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(async () =>
            {
                await _proxyLocalizableStringService.SaveAsync(this.Strings);
            },
            () =>
            {
                return this.Strings != null && this.Strings.Count() > 0;
            }));

        protected override void OnAuthenticationChanged(IPrincipal principal)
        {
            base.OnAuthenticationChanged(principal);

            this.Strings = new List<LocalizableString>();
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}
