using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.ViewModels
{
    internal class CurrentJobWindowViewModel : AuthorizeWindowViewModel, INavigationAware
    {
        const string ALL_ITEMS = "All";

        private readonly ILoggerService _loggerService;
        private readonly ICurrentJobFiltersService _currentJobFiltersService;
        private readonly ICurrentJobStringItemsService _currentJobStringItemsService;
        public CurrentJobWindowViewModel(
            IEventAggregator eventAggregator,
            ILoggerService loggerService,
            ICurrentJobFiltersService currentJobFiltersService,
            ICurrentJobStringItemsService currentJobStringItemsService)
            : base(eventAggregator)
        {
            _loggerService = loggerService;
            _currentJobFiltersService = currentJobFiltersService;
            _currentJobStringItemsService = currentJobStringItemsService;
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

        IEnumerable<StringViewItem> _stringViewItems;
        public IEnumerable<StringViewItem> StringViewItems
        {
            get => _stringViewItems;
            set
            {
                SetProperty<IEnumerable<StringViewItem>>(ref _stringViewItems, value);
            }
        }

        StringViewItem _selectedStringViewItem;
        public StringViewItem SelectedStringViewItem
        {
            get => _selectedStringViewItem;
            set
            {
                SetProperty<StringViewItem>(ref _selectedStringViewItem, value);
            }
        }

        private DelegateCommand _searchCommand = null;
        public DelegateCommand SearchCommand =>
            _searchCommand ?? (_searchCommand = new DelegateCommand(async () =>
            {
                this.GridBusy = true;

                try
                {
                    if (
                        this.SelectedJobItem == null ||
                        this.SelectedComponentNamespace == null ||
                        this.SelectedInternalNamespace == null ||
                        this.SelectedLanguage == null)
                    {
                        this.StringViewItems = null;
                    }
                    else
                    {
                        this.StringViewItems = await _currentJobStringItemsService.GetStringViewItemsAsync(
                            new StringItemViewSearch
                            {
                                ComponentNamespace = this.SelectedComponentNamespace.Description,
                                InternalNamespace = this.SelectedInternalNamespace.Description,
                                ISOCoding = this.SelectedLanguage.ISOCoding,
                                JobListId = this.SelectedJobItem.Id
                            });
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

                this.JobItems = await _currentJobFiltersService.GetJobItemsAsync("marco.delpiano", this.SelectedLanguage != null ? this.SelectedLanguage.ISOCoding : ALL_ITEMS);
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
                this.JobItems = await _currentJobFiltersService.GetJobItemsAsync("marco.delpiano", this.SelectedLanguage != null ? this.SelectedLanguage.ISOCoding : ALL_ITEMS);
                this.ComponentNamespaces = await _currentJobFiltersService.GetComponentNamespacesAsync();
                this.InternalNamespaces = await _currentJobFiltersService.GetInternalNamespacesAsync(this.SelectedComponentNamespace != null ? this.SelectedComponentNamespace.Description : ALL_ITEMS);
                this.Languages = await _currentJobFiltersService.GetLanguagesAsync();

                this.SelectedJobItem = this.JobItems.FirstOrDefault();
                this.SelectedComponentNamespace = this.ComponentNamespaces.FirstOrDefault();
                this.SelectedInternalNamespace = this.InternalNamespaces.FirstOrDefault();
                this.SelectedLanguage = this.Languages.FirstOrDefault(item => item.ISOCoding == "en");
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
