using Globe.Client.Localizer.Dialogs;
using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
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
    internal class JobListManagementWindowViewModel : AuthorizeWindowViewModel, INavigationAware
    {
        const string ALL_ITEMS = "All";

        private readonly ILoggerService _loggerService;
        private readonly IDialogService _dialogService;
        private readonly IJobListManagementFiltersService _jobListManagementFiltersService;
        private readonly IJobListManagementService _jobListManagementService;
        
        public JobListManagementWindowViewModel(
            IEventAggregator eventAggregator,
            ILoggerService loggerService,
            IDialogService dialogService,
            IJobListManagementFiltersService jobListManagementFiltersService,
            IJobListManagementService jobListManagementService
            )
            : base(eventAggregator)
        {
            _loggerService = loggerService;
            _dialogService = dialogService;
            _jobListManagementFiltersService = jobListManagementFiltersService;
            _jobListManagementService = jobListManagementService;
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

        IEnumerable<InternalNamespaceGroup> _internalNamespaceGroups;
        public IEnumerable<InternalNamespaceGroup> InternalNamespaceGroups
        {
            get => _internalNamespaceGroups;
            set
            {
                SetProperty<IEnumerable<InternalNamespaceGroup>>(ref _internalNamespaceGroups, value);
            }
        }

        IEnumerable<NotTranslatedConceptView> _notTranslatedConceptViews;
        public IEnumerable<NotTranslatedConceptView> NotTranslatedConceptViews
        {
            get => _notTranslatedConceptViews;
            set
            {
                SetProperty<IEnumerable<NotTranslatedConceptView>>(ref _notTranslatedConceptViews, value);
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

        bool _filtersBusy;
        public bool FiltersBusy
        {
            get => _filtersBusy;
            set
            {
                SetProperty<bool>(ref _filtersBusy, value);
            }
        }

        private DelegateCommand _languageChangeCommand = null;
        public DelegateCommand LanguageChangeCommand =>
            _languageChangeCommand ?? (_languageChangeCommand = new DelegateCommand(async () =>
            {
                this.FiltersBusy = true;

                this.JobItems = await _jobListManagementFiltersService.GetJobItemsAsync("marco.delpiano", this.SelectedLanguage != null ? this.SelectedLanguage.IsoCoding : ALL_ITEMS);
                this.SelectedJobItem = this.JobItems.FirstOrDefault();

                this.InternalNamespaceGroups = await _jobListManagementService.GetInternalNamespaceGroupsAsync(this.SelectedLanguage);

                this.FiltersBusy = false;
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
                    var result = await _jobListManagementService.GetNotTranslatedConceptsAsync(SelectedInternalNamespaceGroup.ComponentNamespace, SelectedInternalNamespace, this.SelectedLanguage);
                    this.NotTranslatedConceptViews = NotTranslatedConceptViews == null ? result : NotTranslatedConceptViews.Union(result);            
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    ConceptDetailsBusy = false;
                }
            }));

        private DelegateCommand _removeCommand = null;
        public DelegateCommand RemoveCommand =>
            _removeCommand ?? (_removeCommand = new DelegateCommand(() =>
            {
                this.NotTranslatedConceptViews = NotTranslatedConceptViews == null ? null : NotTranslatedConceptViews.Where(item => !item.IsSelected);
            }));

        private DelegateCommand _removeAllCommand = null;
        public DelegateCommand RemoveAllCommand =>
            _removeAllCommand ?? (_removeAllCommand = new DelegateCommand(() =>
            {
                this.NotTranslatedConceptViews = null;
            }));

        private DelegateCommand _saveJoblistCommand = null;
        public DelegateCommand SaveJoblistCommand =>
            _saveJoblistCommand ?? (_saveJoblistCommand = new DelegateCommand(() =>
            {
                var @params = new DialogParameters();

                @params.Add("language", this.SelectedLanguage);
                @params.Add("notTranslatedConceptViews", this.NotTranslatedConceptViews);

                _dialogService.ShowDialog(DialogNames.SAVE_JOBLIST, @params, dialogResult => { });
            }));

        private DelegateCommand _checkNewConceptsCommand = null;
        public DelegateCommand CheckNewConceptsCommand =>
            _checkNewConceptsCommand ?? (_checkNewConceptsCommand = new DelegateCommand(async () =>
            {
                var result = await _jobListManagementService.CheckNewConceptsAsync();
                Console.WriteLine(result);
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
                this.JobItems = await _jobListManagementFiltersService.GetJobItemsAsync("marco.delpiano", this.SelectedLanguage != null ? this.SelectedLanguage.IsoCoding : ALL_ITEMS);
                this.Languages = await _jobListManagementFiltersService.GetLanguagesAsync();

                this.SelectedJobItem = this.JobItems.FirstOrDefault();
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
