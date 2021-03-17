using Globe.Client.Localizer.Dialogs;
using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Assets.Localization;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.ViewModels;
using Globe.Client.Platofrm.Events;
using Globe.Shared.DTOs;
using Globe.Shared.Services;
using Globe.Shared.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.ViewModels
{
    class FiltersUsedBySearching
    {
        public Language Language { get; set; }
        public JobItem JobItem { get; set; }
        public BindableComponentNamespace ComponentNamespace { get; set; }
        public BindableInternalNamespace InternalNamespace { get; set; }
    }

    internal class CurrentJobWindowViewModel : LocalizeWindowViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly ILogService _logService;
        private readonly INotificationService _notificationService;
        private readonly ICurrentJobFiltersService _currentJobFiltersService;
        private readonly ICurrentJobConceptViewService _currentJobConceptViewService;
        private readonly IXmlService _xmlService;
        private readonly IVisibilityFiltersService _visibilityFiltersService;

        public CurrentJobWindowViewModel(
            IIdentityStore identityStore,
            IEventAggregator eventAggregator,
            IDialogService dialogService,
            ILogService logService,
            INotificationService notificationService,
            ICurrentJobFiltersService currentJobFiltersService,
            ICurrentJobConceptViewService currentJobConceptViewService,
            ILocalizationAppService localizationAppService,
            IXmlService xmlService,
            IVisibilityFiltersService visibilityFiltersService)
            : base(identityStore, eventAggregator, localizationAppService)
        {
            _dialogService = dialogService;
            _logService = logService;
            _notificationService = notificationService;
            _currentJobFiltersService = currentJobFiltersService;
            _currentJobConceptViewService = currentJobConceptViewService;
            _xmlService = xmlService;
            _visibilityFiltersService = visibilityFiltersService;
        }

        bool _conceptDetailsBusy;
        public bool ConceptDetailsBusy
        {
            get => _conceptDetailsBusy;
            set
            {
                SetProperty(ref _conceptDetailsBusy, value);
            }
        }

        bool _gridBusy;
        public bool GridBusy
        {
            get => _gridBusy;
            set
            {
                SetProperty(ref _gridBusy, value);
            }
        }

        bool _filtersBusy;
        public bool FiltersBusy
        {
            get => _filtersBusy;
            set
            {
                SetProperty(ref _filtersBusy, value);
            }
        }

        int _itemCount;
        public int ItemCount
        {
            get => _itemCount;
            set
            {
                SetProperty(ref _itemCount, value);
            }
        }

        bool _showFilters = true;
        public bool ShowFilters
        {
            get => _showFilters;
            set
            {
                _visibilityFiltersService.Visible = value;
                SetProperty(ref _showFilters, value);
            }
        }

        IEnumerable<JobItem> _jobItems;
        public IEnumerable<JobItem> JobItems
        {
            get => _jobItems;
            set
            {
                SetProperty(ref _jobItems, value);
            }
        }

        JobItem _selectedJobItem;
        public JobItem SelectedJobItem
        {
            get => _selectedJobItem;
            set
            {
                SetProperty(ref _selectedJobItem, value);
            }
        }

        IEnumerable<BindableComponentNamespace> _componentNamespaces;
        public IEnumerable<BindableComponentNamespace> ComponentNamespaces
        {
            get => _componentNamespaces;
            set
            {
                SetProperty(ref _componentNamespaces, value);
            }
        }

        BindableComponentNamespace _selectedComponentNamespace;
        public BindableComponentNamespace SelectedComponentNamespace
        {
            get => _selectedComponentNamespace;
            set
            {
                SetProperty(ref _selectedComponentNamespace, value);
            }
        }

        IEnumerable<BindableInternalNamespace> _internalNamespaces;
        public IEnumerable<BindableInternalNamespace> InternalNamespaces
        {
            get => _internalNamespaces;
            set
            {
                SetProperty(ref _internalNamespaces, value);
            }
        }

        BindableInternalNamespace _selectedInternalNamespace;
        public BindableInternalNamespace SelectedInternalNamespace
        {
            get => _selectedInternalNamespace;
            set
            {
                SetProperty(ref _selectedInternalNamespace, value);
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

        readonly FiltersUsedBySearching _filtersUsedBySearching = new FiltersUsedBySearching();
        string _filterBy;
        public string FilterBy
        {
            get => _filterBy;
            private set
            {
                SetProperty(ref _filterBy, value);
            }
        }

        IEnumerable<JobListConcept> _conceptViews;
        public IEnumerable<JobListConcept> ConceptViews
        {
            get => _conceptViews;
            set
            {
                SetProperty(ref _conceptViews, value);
            }
        }

        JobListConcept _selectedConceptView;
        public JobListConcept SelectedConceptView
        {
            get => _selectedConceptView;
            set
            {
                SetProperty(ref _selectedConceptView, value);
            }
        }

        private DelegateCommand _searchCommand = null;
        public DelegateCommand SearchCommand =>
            _searchCommand ??= new DelegateCommand(async () =>
            {
                UpdateFiltersUsedBySearching();
                await OnSearch();
            }, () => SelectedJobItem != null);

        private DelegateCommand<JobListConcept> _conceptViewEditCommand = null;
        public DelegateCommand<JobListConcept> ConceptViewEditCommand =>
            _conceptViewEditCommand ??= new DelegateCommand<JobListConcept>(async (conceptView) =>
            {
                ConceptDetailsBusy = true;

                try
                {
                    var conceptDetails = await _currentJobConceptViewService.GetConceptDetailsAsync(conceptView);

                    var @params = new DialogParameters
                    {
                        {
                            DialogParams.EDITABLE_CONCEPT,
                            new EditableConcept(
                        conceptView.Id,
                        conceptView.ComponentNamespace,
                        conceptView.InternalNamespace,
                        conceptView.Name,
                        conceptDetails.SoftwareDeveloperComment,
                        new ObservableCollection<EditableContext>(conceptView.ContextViews.Select(contextView => new EditableContext(conceptDetails.OriginalStringContextValues
                            .Single(item => item.ContextName == contextView.Name).StringValue, contextView.StringInEnglish, contextView.StringValue, contextView.StringId)
                        {
                            ComponentNamespace = conceptView.ComponentNamespace,
                            InternalNamespace = conceptView.InternalNamespace,
                            Concept = conceptView.Name,
                            Name = contextView.Name,
                            Concept2ContextId = contextView.Concept2ContextId,
                            StringType = contextView.StringType,
                            StringId = contextView.StringId,
                        }).ToList()))
                            {
                                MasterTranslatorComment = conceptDetails.MasterTranslatorComment,
                                IgnoreTranslation = conceptDetails.IgnoreTranslation
                            }
                        },
                        { DialogParams.LANGUAGE, _filtersUsedBySearching.Language }
                    };

                    _dialogService.ShowDialog(DialogNames.STRING_EDITOR, @params, async dialogResult =>
                    {
                        if (dialogResult.Result == ButtonResult.OK)
                            await OnSearch();
                    });
                }
                catch (Exception e)
                {
                    _logService.Exception(e);
                    await _notificationService.NotifyAsync(Localize[LanguageKeys.Error], Localize[LanguageKeys.Error_during_concepts_request], Platform.Services.Notifications.NotificationLevel.Error);
                }
                finally
                {
                    ConceptDetailsBusy = false;
                }
            });

        private DelegateCommand _exportToXmlCommand = null;
        public DelegateCommand ExportToXmlCommand =>
            _exportToXmlCommand ??= new DelegateCommand(async () =>
            {
                var downloadPath = ChooseFilePathToDownloadXml();
                if (string.IsNullOrWhiteSpace(downloadPath))
                    return;

                EventAggregator
                .GetEvent<BackgroundBusyChangedEvent>()
                .Publish(true);

                try
                {
                    await _xmlService.Download(downloadPath);
                    await _notificationService.NotifyAsync(Localize[LanguageKeys.Information], Localize[LanguageKeys.Download_completed], Platform.Services.Notifications.NotificationLevel.Info);
                }
                catch (Exception e)
                {
                    _logService.Exception(e);
                    await _notificationService.NotifyAsync(Localize[LanguageKeys.Error], Localize[LanguageKeys.Download_error], Platform.Services.Notifications.NotificationLevel.Error);
                }
                finally
                {
                    EventAggregator
                    .GetEvent<BackgroundBusyChangedEvent>()
                    .Publish(false);
                }
            });

        private DelegateCommand _componentNamespaceChangeCommand = null;
        public DelegateCommand ComponentNamespaceChangeCommand =>
            _componentNamespaceChangeCommand ??= new DelegateCommand(async () =>
            {
                this.FiltersBusy = true;

                this.InternalNamespaces = await _currentJobFiltersService.GetInternalNamespacesAsync(this.SelectedComponentNamespace != null ? this.SelectedComponentNamespace.Description : SharedConstants.COMPONENT_NAMESPACE_ALL);
                this.SelectedInternalNamespace = this.InternalNamespaces.FirstOrDefault();

                this.FiltersBusy = false;
            });

        private DelegateCommand _languageChangeCommand = null;
        public DelegateCommand LanguageChangeCommand =>
            _languageChangeCommand ??= new DelegateCommand(async () =>
            {
                this.FiltersBusy = true;

                this.JobItems = await _currentJobFiltersService.GetJobItemsAsync(this.Identity.Name, this.SelectedLanguage != null ? this.SelectedLanguage.IsoCoding : SharedConstants.LANGUAGE_ALL);
                this.SelectedJobItem = this.JobItems.FirstOrDefault();

                this.FiltersBusy = false;
            });

        async protected override Task OnLoad(object data = null)
        {
            await InitializeFilters(data);
        }

        protected override Task OnUnload()
        {
            ClearPage();
            return Task.CompletedTask;
        }

        async private Task InitializeFilters(object data = null)
        {
            this.FiltersBusy = true;

            try
            {           
                this.JobItems = await _currentJobFiltersService.GetJobItemsAsync(this.Identity.Name, this.SelectedLanguage != null ? this.SelectedLanguage.IsoCoding : SharedConstants.LANGUAGE_ALL);
                this.ComponentNamespaces = await _currentJobFiltersService.GetComponentNamespacesAsync();
                this.InternalNamespaces = await _currentJobFiltersService.GetInternalNamespacesAsync(this.SelectedComponentNamespace != null ? this.SelectedComponentNamespace.Description : SharedConstants.COMPONENT_NAMESPACE_ALL);
                this.Languages = await _currentJobFiltersService.GetLanguagesAsync();

                if(data == null)
                {
                    ShowFilters = _visibilityFiltersService.Visible;

                    this.SelectedJobItem = this.JobItems.FirstOrDefault();
                    this.SelectedComponentNamespace = this.ComponentNamespaces.FirstOrDefault();
                    this.SelectedInternalNamespace = this.InternalNamespaces.FirstOrDefault();
                    this.SelectedLanguage = this.Languages.FirstOrDefault(item => item.IsoCoding == SharedConstants.LANGUAGE_EN);
                }
                else
                {
                    var jobListConceptSearch = (JobListConceptSearch)data;

                    ShowFilters = false;

                    this.SelectedComponentNamespace = this.ComponentNamespaces.FirstOrDefault();
                    this.SelectedInternalNamespace = this.InternalNamespaces.FirstOrDefault();

                    this.SelectedLanguage = this.Languages.Where(item => item.Id == jobListConceptSearch.LanguageId).FirstOrDefault();
                    this.JobItems = await _currentJobFiltersService.GetJobItemsAsync(this.Identity.Name, this.SelectedLanguage.IsoCoding);
                    this.SelectedJobItem = this.JobItems.Where(item => item.Id == jobListConceptSearch.JobListId).FirstOrDefault();

                    this.SearchCommand.Execute();
                }

            }
            catch (Exception e)
            {
                _logService.Exception(e);
            }
            finally
            {
                this.FiltersBusy = false;
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SelectedJobItem))
            {
                SearchCommand.RaiseCanExecuteChanged();
            }
        }
        private async Task OnSearch()
        {
            GridBusy = true;
            ConceptViews = null;
            ItemCount = 0;

            try
            {
                ConceptViews = await _currentJobConceptViewService.GetConceptViewsAsync(
                    new JobListConceptSearch
                    {
                        ComponentNamespace = _filtersUsedBySearching.ComponentNamespace.Description,
                        InternalNamespace = _filtersUsedBySearching.InternalNamespace.Description,
                        LanguageId = _filtersUsedBySearching.Language.Id,
                        JobListId = _filtersUsedBySearching.JobItem.Id
                    });

                ItemCount = ConceptViews.Count();
            }
            catch (OperationCanceledException exception)
            {
                _logService.Exception(exception);
            }
            catch (Exception exception)
            {
                _logService.Exception(exception);
                await _notificationService.NotifyAsync(Localize[LanguageKeys.Error], Localize[LanguageKeys.Error_during_concepts_request], Platform.Services.Notifications.NotificationLevel.Error);
            }
            finally
            {
                this.GridBusy = false;
            }
        }

        private void ClearPage()
        {
            ConceptViews = null;
            ItemCount = 0;
        }

        private static string ChooseFilePathToDownloadXml()
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "xml",
                DefaultExt = ".zip",
                Filter = "Zip documents (.zip)|*.zip"
            };

            if (saveDialog.ShowDialog() != true)
                return null;

            return saveDialog.FileName;
        }

        private void UpdateFiltersUsedBySearching()
        {
            _filtersUsedBySearching.Language = SelectedLanguage;
            _filtersUsedBySearching.JobItem = SelectedJobItem;
            _filtersUsedBySearching.ComponentNamespace = SelectedComponentNamespace;
            _filtersUsedBySearching.InternalNamespace = SelectedInternalNamespace;

            FilterBy = $"{Localize["FilterBy"]} {_filtersUsedBySearching?.Language?.Name}, {Localize["JobListName"]}: {_filtersUsedBySearching?.JobItem?.Name}";
        }
    }
}
