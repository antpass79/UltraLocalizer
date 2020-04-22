using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.ViewModels;
using Globe.Client.Platofrm.Events;
using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Globe.Client.Localizer.ViewModels
{
    internal class MergeWindowViewModel : AuthorizeWindowViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IAsyncLocalizableStringService _fileSystemLocalizableStringService;
        private readonly IAsyncLocalizableStringService _httpLocalizableStringService;
        private readonly IStringMergeService _stringMergeService;
        private readonly ILocalizationService _localizationService;

        public MergeWindowViewModel(
            IEventAggregator eventAggregator,
            IFileSystemLocalizableStringService fileSystemLocalizableStringService,
            IHttpLocalizableStringService httpLocalizableStringService,
            IStringMergeService stringMergeService,
            ILocalizationService localizationService)
            : base(eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _fileSystemLocalizableStringService = fileSystemLocalizableStringService;
            _httpLocalizableStringService = httpLocalizableStringService;
            _stringMergeService = stringMergeService;
            _localizationService = localizationService;
        }

        IEnumerable<LocalizableString> _fileSystemStrings;
        public IEnumerable<LocalizableString> FileSystemStrings
        {
            get => _fileSystemStrings;
            set
            {
                SetProperty<IEnumerable<LocalizableString>>(ref _fileSystemStrings, value);
            }
        }

        IEnumerable<LocalizableString> _httpStrings;
        public IEnumerable<LocalizableString> HttpStrings
        {
            get => _httpStrings;
            set
            {
                SetProperty<IEnumerable<LocalizableString>>(ref _httpStrings, value);
            }
        }

        IEnumerable<LocalizableString> _mergedStrings;
        public IEnumerable<LocalizableString> MergedStrings
        {
            get => _mergedStrings;
            set
            {
                SetProperty<IEnumerable<LocalizableString>>(ref _mergedStrings, value);
            }
        }

        private DelegateCommand _loadCommand = null;
        public DelegateCommand LoadCommand =>
            _loadCommand ?? (_loadCommand = new DelegateCommand(async () =>
            {
                this.FileSystemStrings = await _fileSystemLocalizableStringService.GetAllAsync();
                this.HttpStrings = await _httpLocalizableStringService.GetAllAsync();
                AutoMergeCommand.RaiseCanExecuteChanged();
            }));

        private DelegateCommand _autoMergeCommand = null;
        public DelegateCommand AutoMergeCommand =>
            _autoMergeCommand ?? (_autoMergeCommand = new DelegateCommand(() =>
            {
                this.MergedStrings = _stringMergeService.Merge(this.FileSystemStrings, this.HttpStrings);
                SaveCommand.RaiseCanExecuteChanged();
            },
            () =>
            {
                return this.FileSystemStrings != null && this.HttpStrings != null && (this.FileSystemStrings.Count() > 0 || this.HttpStrings.Count() > 0);
            }));

        private DelegateCommand _saveCommand = null;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(async () =>
            {
                _eventAggregator.GetEvent<BusyChangedEvent>().Publish(true);

                try
                {
                    await _httpLocalizableStringService.SaveAsync(this.MergedStrings);
                    _eventAggregator.GetEvent<StatusBarMessageChangedEvent>().Publish(new StatusBarMessage
                    {
                        MessageType = MessageType.Information,
                        Text = _localizationService.Resolve("Operation_successfully_completed")
                    });
                }
                catch
                {
                    _eventAggregator.GetEvent<StatusBarMessageChangedEvent>().Publish(new StatusBarMessage
                    {
                        MessageType = MessageType.Error,
                        Text = _localizationService.Resolve("Error_during_server_communication")
                    });
                }
                finally
                {
                    _eventAggregator.GetEvent<BusyChangedEvent>().Publish(false);
                }
            },
            () =>
            {
                return this.MergedStrings != null && this.MergedStrings.Count() > 0;
            }));

        private DelegateCommand<LocalizableString> _selectFromFileSystemCommand = null;
        public DelegateCommand<LocalizableString> SelectFromFileSystemCommand =>
            _selectFromFileSystemCommand ?? (_selectFromFileSystemCommand = new DelegateCommand<LocalizableString>((localizableString) =>
            {
                if (MergedStrings == null)
                    return;

                foreach (var item in MergedStrings)
                {
                    if (localizableString.Key == item.Key)
                    {
                        item.SavedOn = localizableString.SavedOn;
                        item.Value = localizableString.Value;

                        break;
                    }
                }

                MergedStrings = MergedStrings.ToList();
            }));
        
        private DelegateCommand<LocalizableString> _selectFromServerCommand = null;
        public DelegateCommand<LocalizableString> SelectFromServerCommand =>
            _selectFromServerCommand ?? (_selectFromServerCommand = new DelegateCommand<LocalizableString>((localizableString) =>
            {
                if (MergedStrings == null)
                    return;

                foreach (var item in MergedStrings)
                {
                    if (localizableString.Key == item.Key)
                    {
                        item.SavedOn = localizableString.SavedOn;
                        item.Value = localizableString.Value;

                        break;
                    }
                }

                MergedStrings = MergedStrings.ToList();
            }));

        protected override void OnAuthenticationChanged(IPrincipal principal)
        {
            base.OnAuthenticationChanged(principal);

            this.FileSystemStrings = new List<LocalizableString>();
            this.HttpStrings = new List<LocalizableString>();
            this.MergedStrings = new List<LocalizableString>();
            AutoMergeCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}
