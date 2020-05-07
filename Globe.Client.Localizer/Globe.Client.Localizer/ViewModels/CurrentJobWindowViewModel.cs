using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.ViewModels;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.ViewModels
{
    internal class CurrentJobWindowViewModel : AuthorizeWindowViewModel
    {
        const string ALL_ITEMS = "All";

        private readonly ICurrentJobFiltersService _currentJobFiltersService;
        private readonly ICurrentJobStringItemsService _currentJobStringItemsService;
        public CurrentJobWindowViewModel(
            IEventAggregator eventAggregator,
            ICurrentJobFiltersService currentJobFiltersService,
            ICurrentJobStringItemsService currentJobStringItemsService)
            : base(eventAggregator)
        {
            _currentJobFiltersService = currentJobFiltersService;
            _currentJobStringItemsService = currentJobStringItemsService;

            InitializeFilters();
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

        IEnumerable<StringItemView> _stringItemViews;
        public IEnumerable<StringItemView> StringItemViews
        {
            get => _stringItemViews;
            set
            {
                SetProperty<IEnumerable<StringItemView>>(ref _stringItemViews, value);
            }
        }

        StringItemView _selectedStringItemView;
        public StringItemView SelectedStringItemView
        {
            get => _selectedStringItemView;
            set
            {
                SetProperty<StringItemView>(ref _selectedStringItemView, value);
            }
        }

        private DelegateCommand _searchCommand = null;
        public DelegateCommand SearchCommand =>
            _searchCommand ?? (_searchCommand = new DelegateCommand(async () =>
            {
                this.StringItemViews = await _currentJobStringItemsService.GetStringItemsAsync(
                    new StringItemViewSearch
                    {
                        ComponentNamespace = this.SelectedComponentNamespace.Description,
                        InternalNamespace = this.SelectedInternalNamespace.Description,
                        ISOCoding = this.SelectedLanguage.ISOCoding,
                        JobListId = this.SelectedJobItem.Id
                    });
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

        private DelegateCommand _componentNamespaceChangeCommand = null;
        public DelegateCommand ComponentNamespaceChangeCommand =>
            _componentNamespaceChangeCommand ?? (_componentNamespaceChangeCommand = new DelegateCommand(async () =>
            {
                this.InternalNamespaces = await _currentJobFiltersService.GetInternalNamespacesAsync(this.SelectedComponentNamespace != null ? this.SelectedComponentNamespace.Description : ALL_ITEMS);
            }));

        private DelegateCommand _languageChangeCommand = null;
        public DelegateCommand LanguageChangeCommand =>
            _languageChangeCommand ?? (_languageChangeCommand = new DelegateCommand(async () =>
            {
                this.JobItems = await _currentJobFiltersService.GetJobItemsAsync("marco.delpiano", this.SelectedLanguage != null ? this.SelectedLanguage.ISOCoding : ALL_ITEMS);
            }));

        async private Task InitializeFilters()
        {
            this.JobItems = await _currentJobFiltersService.GetJobItemsAsync("marco.delpiano", this.SelectedLanguage != null ? this.SelectedLanguage.ISOCoding : ALL_ITEMS);
            this.ComponentNamespaces = await _currentJobFiltersService.GetComponentNamespacesAsync();
            this.InternalNamespaces = await _currentJobFiltersService.GetInternalNamespacesAsync(this.SelectedComponentNamespace != null ? this.SelectedComponentNamespace.Description : ALL_ITEMS);
            this.Languages = await _currentJobFiltersService.GetLanguagesAsync();
        }
    }
}
