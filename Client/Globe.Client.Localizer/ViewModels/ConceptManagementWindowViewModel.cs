using Globe.Client.Localizer.Dialogs;
using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.ViewModels;
using Globe.Shared.DTOs;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.ViewModels
{
    internal class ConceptManagementWindowViewModel : LocalizeWindowViewModel
    {
        const string ALL_ITEMS = "All";

        private readonly ILoggerService _loggerService;
        private readonly INotificationService _notificationService;
        private readonly IConceptManagementFiltersService _conceptManagementFiltersService;
        private readonly IConceptManagementViewService _conceptManagementViewService;

        public ConceptManagementWindowViewModel(
            IEventAggregator eventAggregator,
            ILoggerService loggerService,
            INotificationService notificationService,
            IConceptManagementFiltersService conceptManagementFiltersService,
            IConceptManagementViewService conceptManagementViewService,
            ILocalizationAppService localizationAppService)
            : base(eventAggregator, localizationAppService)
        {
            _loggerService = loggerService;
            _notificationService = notificationService;
            _conceptManagementFiltersService = conceptManagementFiltersService;
            _conceptManagementViewService = conceptManagementViewService;
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

        string _stringInserted = string.Empty;
        public string StringInserted
        {
            get => _stringInserted;
            set 
            { 
                SetProperty(ref _stringInserted, value); 
            }
        }

        string _conceptInserted = string.Empty;
        public string ConceptInserted
        {
            get => _conceptInserted;
            set
            {
                SetProperty(ref _conceptInserted, value);
            }
        }

        IEnumerable<Context> _contexts;
        public IEnumerable<Context> Contexts
        {
            get => _contexts;
            set
            {
                SetProperty(ref _contexts, value);
            }
        }

        Context _selectedContext;
        public Context SelectedContext
        {
            get => _selectedContext;
            set
            {
                SetProperty(ref _selectedContext, value);
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

        IEnumerable<ConceptTranslated> _conceptViews;
        public IEnumerable<ConceptTranslated> ConceptViews
        {
            get => _conceptViews;
            set
            {
                SetProperty(ref _conceptViews, value);
            }
        }

        private DelegateCommand _searchCommand = null;
        public DelegateCommand SearchCommand =>
            _searchCommand ?? (_searchCommand = new DelegateCommand(async () =>
            {
                await OnSearch();
            }));

        private DelegateCommand _componentNamespaceChangeCommand = null;
        public DelegateCommand ComponentNamespaceChangeCommand =>
            _componentNamespaceChangeCommand ?? (_componentNamespaceChangeCommand = new DelegateCommand(async () =>
            {
                this.FiltersBusy = true;

                this.InternalNamespaces = await _conceptManagementFiltersService.GetInternalNamespacesAsync(this.SelectedComponentNamespace != null ? this.SelectedComponentNamespace.Description : ALL_ITEMS);
                this.SelectedInternalNamespace = this.InternalNamespaces.FirstOrDefault();

                this.FiltersBusy = false;
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

        async private Task InitializeFilters()
        {
            this.FiltersBusy = true;

            try
            {
                this.Contexts = await _conceptManagementFiltersService.GetContextAsync();
                this.ComponentNamespaces = await _conceptManagementFiltersService.GetComponentNamespacesAsync();
                this.InternalNamespaces = await _conceptManagementFiltersService.GetInternalNamespacesAsync(this.SelectedComponentNamespace != null ? this.SelectedComponentNamespace.Description : ALL_ITEMS);
                this.Languages = await _conceptManagementFiltersService.GetLanguagesAsync();

                this.SelectedContext = this.Contexts.FirstOrDefault();
                this.SelectedComponentNamespace = this.ComponentNamespaces.FirstOrDefault();
                this.SelectedInternalNamespace = this.InternalNamespaces.FirstOrDefault();
                this.SelectedLanguage = this.Languages.FirstOrDefault();
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

        private async Task OnSearch()
        {
            GridBusy = true;
            ConceptViews = null;
            ItemCount = 0;

            try
            {
                if (
                    SelectedContext == null ||
                    SelectedComponentNamespace == null ||
                    SelectedInternalNamespace == null ||
                    SelectedLanguage == null)
                {
                    ConceptViews = null;
                }
                else
                {
                    ConceptViews = await _conceptManagementViewService.GetConceptViewsAsync(
                        new ConceptManagementSearch
                        {
                            ComponentNamespace = SelectedComponentNamespace.Description,
                            InternalNamespace = SelectedInternalNamespace.Description,
                            LanguageId = SelectedLanguage.Id,
                            Context = SelectedContext.Name,
                            Concept = ConceptInserted,
                            LocalizedString = StringInserted
                        });

                    ItemCount = ConceptViews.Count();
                }
            }
            catch (Exception exception)
            {
                _loggerService.Exception(exception);
                await _notificationService.NotifyAsync("Error", "Error during concepts request", Platform.Services.Notifications.NotificationLevel.Error);
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
    }
}
