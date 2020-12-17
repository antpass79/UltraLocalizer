using Globe.Client.Localizer.Dialogs;
using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.Services.Notifications;
using Globe.Client.Platform.Utilities;
using Globe.Client.Platform.ViewModels;
using Globe.Client.Platofrm.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.ViewModels
{
    internal class JobListManagementWindowViewModel : LocalizeWindowViewModel
    {
        private readonly ILoggerService _loggerService;
        private readonly IDialogService _dialogService;
        private readonly IJobListManagementFiltersService _jobListManagementFiltersService;
        private readonly IJobListManagementService _jobListManagementService;
        private readonly INotificationService _notificationService;


        public JobListManagementWindowViewModel(
            IEventAggregator eventAggregator,
            ILoggerService loggerService,
            IDialogService dialogService,
            IJobListManagementFiltersService jobListManagementFiltersService,
            IJobListManagementService jobListManagementService,
            ILocalizationAppService localizationAppService,
            INotificationService notificationService
            )
            : base(eventAggregator, localizationAppService)
        {
            _loggerService = loggerService;
            _dialogService = dialogService;
            _jobListManagementFiltersService = jobListManagementFiltersService;
            _jobListManagementService = jobListManagementService;
            _notificationService = notificationService;

        }

        bool _componentsVisible;
        public bool ComponentsVisible
        {
            get => _componentsVisible;
            private set
            {
                SetProperty(ref _componentsVisible, value);
            }
        }

        int _ItemCount;
        public int ItemCount
        {
            get => _ItemCount;
            set
            {
                SetProperty(ref _ItemCount, value);
            }
        }

        IEnumerable<Language> _languages;
        public IEnumerable<Language> Languages
        {
            get => _languages;
            set
            {
                SetProperty(ref _languages, value);
            }
        }

        Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                SetProperty(ref _selectedLanguage, value);
            }
        }

        NotTranslatedConceptView _selectedNotTranslatedConceptView;
        public NotTranslatedConceptView SelectedNotTranslatedConceptView
        {
            get => _selectedNotTranslatedConceptView;
            set
            {
                SetProperty(ref _selectedNotTranslatedConceptView, value);
            }
        }

        IEnumerable<InternalNamespaceGroup> _internalNamespaceGroups;
        public IEnumerable<InternalNamespaceGroup> InternalNamespaceGroups
        {
            get => _internalNamespaceGroups;
            set
            {
                SetProperty(ref _internalNamespaceGroups, value);
            }
        }

        IEnumerable<NotTranslatedConceptView> _notTranslatedConceptViews;
        public IEnumerable<NotTranslatedConceptView> NotTranslatedConceptViews
        {
            get => _notTranslatedConceptViews;
            set
            {
                SetProperty(ref _notTranslatedConceptViews, value);
            }
        }

        public InternalNamespace SelectedInternalNamespace
        {
            get
            {
                if (InternalNamespaceGroups == null)
                    return null;

                return InternalNamespaceGroups
                    .SelectMany(item => item.InternalNamespaces)
                    .SingleOrDefault(item => item.IsSelected);
            }
        }

        public InternalNamespaceGroup SelectedInternalNamespaceGroup
        {
            get
            {
                var selectedInternalNamespace = SelectedInternalNamespace;

                if (selectedInternalNamespace == null)
                    return null;

                return InternalNamespaceGroups
                    .SingleOrDefault(item => item.InternalNamespaces.Contains(selectedInternalNamespace));
            }
        }

        SmartBusy _smartConceptDetailsBusy = new SmartBusy();
        bool _conceptDetailsBusy;
        public bool ConceptDetailsBusy
        {
            get => _smartConceptDetailsBusy.Busy;
            set
            {
                _smartConceptDetailsBusy.Busy = value;
                SetProperty(ref _conceptDetailsBusy, _smartConceptDetailsBusy.Busy);
            }
        }

        SmartBusy _smartFiltersBusy = new SmartBusy();
        bool _filtersBusy;
        public bool FiltersBusy
        {
            get => _smartFiltersBusy.Busy;
            set
            {
                _smartFiltersBusy.Busy = value;
                SetProperty(ref _filtersBusy, _smartFiltersBusy.Busy);
            }
        }

        private DelegateCommand _languageChangeCommand = null;
        public DelegateCommand LanguageChangeCommand =>
            _languageChangeCommand ?? (_languageChangeCommand = new DelegateCommand(async () =>
            {
                await OnLanguageChange();
                ItemCount = 0;
            }));

        private DelegateCommand _addCommand = null;
        public DelegateCommand AddCommand =>
            _addCommand ?? (_addCommand = new DelegateCommand(async () =>
            {
                if (SelectedInternalNamespaceGroup == null || SelectedInternalNamespace == null)
                    return;

                try
                {
                    ConceptDetailsBusy = true;
                    var result = await _jobListManagementService.GetNotTranslatedConceptsAsync(SelectedInternalNamespaceGroup.ComponentNamespace, SelectedInternalNamespace, SelectedLanguage);

                    if (NotTranslatedConceptViews == null)
                    {
                        NotTranslatedConceptViews = result;
                    }
                    else
                    {
                        var newElements = result.Where(item => !NotTranslatedConceptViews.Any(gridItem => gridItem.Name == item.Name)).ToList();
                        if (newElements != null)
                        {
                            NotTranslatedConceptViews = NotTranslatedConceptViews.Union(newElements)
                            .OrderBy(item => item.ComponentNamespace.Description)
                            .ThenBy(item => item.InternalNamespace.Description)
                            .ThenBy(item => item.Name)
                            .ToList();
                        }
                    }
                }
                catch (Exception e)
                {
                    _loggerService.Exception(e);
                    await _notificationService.NotifyAsync(new Notification
                    {
                        Title = "Error",
                        Message = "Error during getting list",
                        Level = NotificationLevel.Error
                    });
                }
                finally
                {
                    ConceptDetailsBusy = false;
                    ItemCount = NotTranslatedConceptViews == null ? 0 : NotTranslatedConceptViews.Count();
                }
            }, () => !FiltersBusy));

        private DelegateCommand _removeCommand = null;
        public DelegateCommand RemoveCommand =>
            _removeCommand ?? (_removeCommand = new DelegateCommand(() =>
            {
                NotTranslatedConceptViews = NotTranslatedConceptViews == null ? null : NotTranslatedConceptViews.Where(item => !item.IsSelected);
                ItemCount = NotTranslatedConceptViews == null ? 0 : NotTranslatedConceptViews.Count();
            }, () => SelectedNotTranslatedConceptView != null));

        private DelegateCommand _removeAllCommand = null;
        public DelegateCommand RemoveAllCommand =>
            _removeAllCommand ?? (_removeAllCommand = new DelegateCommand(() =>
            {
                NotTranslatedConceptViews = null;
                ItemCount = 0;
            }, () => NotTranslatedConceptViews != null && NotTranslatedConceptViews.Count() > 0));

        private DelegateCommand _saveJoblistCommand = null;
        public DelegateCommand SaveJoblistCommand =>
            _saveJoblistCommand ?? (_saveJoblistCommand = new DelegateCommand(() =>
            {
                var @params = new DialogParameters();

                @params.Add("language", SelectedLanguage);
                @params.Add("notTranslatedConceptViews", NotTranslatedConceptViews);

                _dialogService.ShowDialog(DialogNames.SAVE_JOBLIST, @params, dialogResult =>
                {
                    if (dialogResult.Result == ButtonResult.OK)
                    {
                        NotTranslatedConceptViews = null;
                        ItemCount = 0;
                    }
                });
            }, () => NotTranslatedConceptViews != null && NotTranslatedConceptViews.Count() > 0));

        private DelegateCommand _checkNewConceptsCommand = null;
        public DelegateCommand CheckNewConceptsCommand =>
            _checkNewConceptsCommand ?? (_checkNewConceptsCommand = new DelegateCommand(async () =>
            {
                try
                {
                    EventAggregator.GetEvent<BusyChangedEvent>().Publish(true);
                    var result = await _jobListManagementService.CheckNewConceptsAsync();

                    await _notificationService.NotifyAsync(new Notification
                    {
                        Title = "Get new concepts",
                        Message = result ? "New Concepts Found" : "No Concepts Found",
                        Level = NotificationLevel.Info
                    });
                    await OnLanguageChange();
                }
                catch (Exception e)
                {
                    _loggerService.Exception(e);
                    await _notificationService.NotifyAsync(new Notification
                    {
                        Title = "Error: Get new concepts",
                        Message = "Error during searching new Concepts",
                        Level = NotificationLevel.Error
                    });
                }
                finally
                {
                    EventAggregator.GetEvent<BusyChangedEvent>().Publish(false);
                }
            }));

        async protected override Task OnLoad()
        {
            await InitializeFilters();
        }

        protected override Task OnUnload()
        {
            ClearPage();
            return Task.CompletedTask;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(FiltersBusy))
            {
                AddCommand.RaiseCanExecuteChanged();
            }

            if (args.PropertyName == nameof(NotTranslatedConceptViews))
            {
                SaveJoblistCommand.RaiseCanExecuteChanged();
                RemoveAllCommand.RaiseCanExecuteChanged();
            }

            if (args.PropertyName == nameof(SelectedNotTranslatedConceptView))
            {
                RemoveCommand.RaiseCanExecuteChanged();
            }
        }

        async private Task InitializeFilters()
        {
            try
            {
                FiltersBusy = true;
                Languages = await _jobListManagementFiltersService.GetLanguagesAsync();
                SelectedLanguage = Languages.FirstOrDefault(item => item.IsoCoding == "en");
            }
            catch (Exception e)
            {
                _loggerService.Exception(e);
                await _notificationService.NotifyAsync(new Notification
                {
                    Title = "Error",
                    Message = "Error during searching new Concepts",
                    Level = NotificationLevel.Error
                });
            }
            finally
            {
                FiltersBusy = false;
            }
        }

        private async Task OnLanguageChange()
        {
            if (SelectedLanguage == null)
                return;

            try
            {
                FiltersBusy = true;

                InternalNamespaceGroups = await _jobListManagementService.GetInternalNamespaceGroupsAsync(SelectedLanguage);
                ComponentsVisible = InternalNamespaceGroups != null && InternalNamespaceGroups.Count() > 0;
                NotTranslatedConceptViews = null;
            }
            catch (Exception e)
            {
                _loggerService.Exception(e);

                NotTranslatedConceptViews = null;
                InternalNamespaceGroups = null;
                await _notificationService.NotifyAsync(new Notification
                {
                    Title = "Error",
                    Message = "Error during building groups",
                    Level = NotificationLevel.Error
                });
            }
            finally
            {
                FiltersBusy = false;
            }
        }

        private void ClearPage()
        {
            NotTranslatedConceptViews = null;
            InternalNamespaceGroups = null;
            ItemCount = 0;
        }
    }
}
