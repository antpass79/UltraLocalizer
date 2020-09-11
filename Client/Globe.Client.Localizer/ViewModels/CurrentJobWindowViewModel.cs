using Globe.Client.Localizer.Dialogs;
using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.ViewModels
{
    internal class CurrentJobWindowViewModel : AuthorizeWindowViewModel, INavigationAware
    {
        const string ALL_ITEMS = "All";

        private readonly IDialogService _dialogService;
        private readonly ILoggerService _loggerService;
        private readonly ICurrentJobFiltersService _currentJobFiltersService;
        private readonly ICurrentJobConceptViewService _currentJobConceptViewService;
        public CurrentJobWindowViewModel(
            IEventAggregator eventAggregator,
            IDialogService dialogService,
            ILoggerService loggerService,
            ICurrentJobFiltersService currentJobFiltersService,
            ICurrentJobConceptViewService currentJobConceptViewService)
            : base(eventAggregator)
        {
            _dialogService = dialogService;
            _loggerService = loggerService;
            _currentJobFiltersService = currentJobFiltersService;
            _currentJobConceptViewService = currentJobConceptViewService;
        }

        WorkingMode _workingMode;
        public WorkingMode WorkingMode
        {
            get => _workingMode;
            set
            {
                SetProperty<WorkingMode>(ref _workingMode, value);
            }
        }

        bool _conceptDetailsBusy;
        public bool ConceptDetailsBusy
        {
            get => _conceptDetailsBusy;
            set
            {
                SetProperty<bool>(ref _conceptDetailsBusy, value);
            }
        }

        bool _gridBusy;
        public bool GridBusy
        {
            get => _gridBusy;
            set
            {
                SetProperty<bool>(ref _gridBusy, value);
            }
        }

        bool _filtersBusy;
        public bool FiltersBusy
        {
            get => _filtersBusy;
            set
            {
                SetProperty<bool>(ref _filtersBusy, value);
            }
        }

        int _numRows;

        public int NumRows
        {
            get => _numRows;
            set
            {
                SetProperty<int>(ref _numRows, value);
            }
        }



        IEnumerable<JobItem> _jobItems;
        public IEnumerable<JobItem> JobItems
        {
            get => _jobItems;
            set
            {
                SetProperty<IEnumerable<JobItem>>(ref _jobItems, value);
            }
        }

        JobItem _selectedJobItem;
        public JobItem SelectedJobItem
        {
            get => _selectedJobItem;
            set
            {
                SetProperty<JobItem>(ref _selectedJobItem, value);
            }
        }

        IEnumerable<ComponentNamespace> _componentNamespaces;
        public IEnumerable<ComponentNamespace> ComponentNamespaces
        {
            get => _componentNamespaces;
            set
            {
                SetProperty<IEnumerable<ComponentNamespace>>(ref _componentNamespaces, value);
            }
        }

        ComponentNamespace _selectedComponentNamespace;
        public ComponentNamespace SelectedComponentNamespace
        {
            get => _selectedComponentNamespace;
            set
            {
                SetProperty<ComponentNamespace>(ref _selectedComponentNamespace, value);
            }
        }

        IEnumerable<InternalNamespace> _internalNamespaces;
        public IEnumerable<InternalNamespace> InternalNamespaces
        {
            get => _internalNamespaces;
            set
            {
                SetProperty<IEnumerable<InternalNamespace>>(ref _internalNamespaces, value);
            }
        }

        InternalNamespace _selectedInternalNamespace;
        public InternalNamespace SelectedInternalNamespace
        {
            get => _selectedInternalNamespace;
            set
            {
                SetProperty<InternalNamespace>(ref _selectedInternalNamespace, value);
            }
        }

        IEnumerable<Language> _languages;
        public IEnumerable<Language> Languages
        {
            get => _languages;
            set
            {
                SetProperty<IEnumerable<Language>>(ref _languages, value);
            }
        }

        Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                SetProperty<Language>(ref _selectedLanguage, value);
            }
        }

        IEnumerable<ConceptView> _conceptViews;
        public IEnumerable<ConceptView> ConceptViews
        {
            get => _conceptViews;
            set
            {
                SetProperty<IEnumerable<ConceptView>>(ref _conceptViews, value);
            }
        }

        ConceptView _selectedConceptView;
        public ConceptView SelectedConceptView
        {
            get => _selectedConceptView;
            set
            {
                SetProperty<ConceptView>(ref _selectedConceptView, value);
            }
        }

        private DelegateCommand _searchCommand = null;
        public DelegateCommand SearchCommand =>
            _searchCommand ?? (_searchCommand = new DelegateCommand(async () =>
            {
                this.GridBusy = true;
                this.ConceptViews = null;
                this.NumRows = 0;

                try
                {
                    if (
                        this.SelectedJobItem == null ||
                        this.SelectedComponentNamespace == null ||
                        this.SelectedInternalNamespace == null ||
                        this.SelectedLanguage == null)
                    {
                        this.ConceptViews = null;
                    }
                    else
                    {
                        this.ConceptViews = await _currentJobConceptViewService.GetConceptViewsAsync(
                            new ConceptViewSearch
                            {
                                ComponentNamespace = this.SelectedComponentNamespace.Description,
                                InternalNamespace = this.SelectedInternalNamespace.Description,
                                LanguageId = this.SelectedLanguage.Id,
                                JobItemId = this.SelectedJobItem.Id,
                                WorkingMode = this.WorkingMode
                            });

                        this.NumRows = ConceptViews.Count();
                    }
                }
                catch (Exception exception)
                {
                    _loggerService.Exception(exception);
                }
                finally
                {
                    this.GridBusy = false;                  
                }
            }));
        
        private DelegateCommand<ConceptView> _conceptViewEditCommand = null;
        public DelegateCommand<ConceptView> ConceptViewEditCommand =>
            _conceptViewEditCommand ?? (_conceptViewEditCommand = new DelegateCommand<ConceptView>((conceptView) =>
            {
                ConceptDetails conceptDetails = new ConceptDetails();

                ConceptDetailsBusy = true;

                try
                {
                    conceptDetails = _currentJobConceptViewService.GetConceptDetailsAsync(conceptView).RunSync();
                }
                catch (Exception e)
                {
                    _loggerService.Exception(e);
                }
                finally
                {
                    ConceptDetailsBusy = false;
                }

                var @params = new DialogParameters();

                @params.Add("editableConcept", new EditableConcept(
                    conceptView.Id,
                    conceptView.ComponentNamespace,
                    conceptView.InternalNamespace,
                    conceptView.Name,
                    conceptDetails.SoftwareDeveloperComment,
                    new ObservableCollection<EditableContext>(conceptView.ContextViews.Select(contextView => new EditableContext(contextView.StringValue, contextView.OldStringId)
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
                    MasterTranslatorComment = conceptDetails.MasterTranslatorComment
                });
                @params.Add("language", this.SelectedLanguage);

                _dialogService.ShowDialog(DialogNames.STRING_EDITOR, @params, dialogResult => { });
            }));

        private DelegateCommand _checkNewJobCommand = null;
        public DelegateCommand CheckNewJobCommand =>
            _checkNewJobCommand ?? (_checkNewJobCommand = new DelegateCommand(() =>
            {
            }));

        private DelegateCommand _exportToXmlCommand = null;
        public DelegateCommand ExportToXmlCommand =>
            _exportToXmlCommand ?? (_exportToXmlCommand = new DelegateCommand(() =>
            {
            }));

        private DelegateCommand<WorkingMode?> _workingModeCommand = null;
        public DelegateCommand<WorkingMode?> WorkingModeCommand =>
            _workingModeCommand ?? (_workingModeCommand = new DelegateCommand<WorkingMode?>((workingMode) =>
            {
                this.WorkingMode = workingMode.HasValue ? workingMode.Value : WorkingMode.FromXml;
            }));

        private DelegateCommand _componentNamespaceChangeCommand = null;
        public DelegateCommand ComponentNamespaceChangeCommand =>
            _componentNamespaceChangeCommand ?? (_componentNamespaceChangeCommand = new DelegateCommand(async () =>
            {
                this.FiltersBusy = true;

                this.InternalNamespaces = await _currentJobFiltersService.GetInternalNamespacesAsync(this.SelectedComponentNamespace != null ? this.SelectedComponentNamespace.Description : ALL_ITEMS);
                this.SelectedInternalNamespace = this.InternalNamespaces.FirstOrDefault();

                this.FiltersBusy = false;
            }));

        private DelegateCommand _languageChangeCommand = null;
        public DelegateCommand LanguageChangeCommand =>
            _languageChangeCommand ?? (_languageChangeCommand = new DelegateCommand(async () =>
            {
                this.FiltersBusy = true;

                this.JobItems = await _currentJobFiltersService.GetJobItemsAsync("marco.delpiano", this.SelectedLanguage != null ? this.SelectedLanguage.IsoCoding : ALL_ITEMS);
                this.SelectedJobItem = this.JobItems.FirstOrDefault();

                this.FiltersBusy = false;
            }));

        async public void OnNavigatedTo(NavigationContext navigationContext)
        {
            await InitializeFilters();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        async private Task InitializeFilters()
        {
            this.FiltersBusy = true;

            try
            {
                this.JobItems = await _currentJobFiltersService.GetJobItemsAsync("marco.delpiano", this.SelectedLanguage != null ? this.SelectedLanguage.IsoCoding : ALL_ITEMS);
                this.ComponentNamespaces = await _currentJobFiltersService.GetComponentNamespacesAsync();
                this.InternalNamespaces = await _currentJobFiltersService.GetInternalNamespacesAsync(this.SelectedComponentNamespace != null ? this.SelectedComponentNamespace.Description : ALL_ITEMS);
                this.Languages = await _currentJobFiltersService.GetLanguagesAsync();

                this.SelectedJobItem = this.JobItems.FirstOrDefault();
                this.SelectedComponentNamespace = this.ComponentNamespaces.FirstOrDefault();
                this.SelectedInternalNamespace = this.InternalNamespaces.FirstOrDefault();
                this.SelectedLanguage = this.Languages.FirstOrDefault(item => item.IsoCoding == "en");
            }
            catch (Exception e)
            {
                _loggerService.Exception(e);
            }
            finally
            {
                this.FiltersBusy = false;
            }
        }
    }
}
