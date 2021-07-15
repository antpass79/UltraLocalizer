using MyLabLocalizer.Models;
using MyLabLocalizer.Services;
using MyLabLocalizer.Core.Services;
using MyLabLocalizer.Core.ViewModels;
using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace MyLabLocalizer.ViewModels
{
    internal class JobsWindowViewModel : AuthorizeWindowViewModel
    {

        private readonly IAsyncLocalizableStringService _proxyLocalizableStringService;

        public JobsWindowViewModel(
            IIdentityStore identityStore,
            IEventAggregator eventAggregator,
            IProxyLocalizableStringService proxyLocalizableStringService,
            IStringMergeService stringMergeService)
            : base(identityStore, eventAggregator)
        {
            _proxyLocalizableStringService = proxyLocalizableStringService;
        }

        IEnumerable<LocalizableString> _strings;
        public IEnumerable<LocalizableString> Strings
        {
            get => _strings;
            set
            {
                SetProperty(ref _strings, value);
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
