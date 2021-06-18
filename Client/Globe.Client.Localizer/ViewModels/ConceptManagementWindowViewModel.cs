using Globe.Client.Localizer.Dialogs;
using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Assets.Localization;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.Services.Notifications;
using Globe.Client.Platform.ViewModels;
using Globe.Shared.DTOs;
using Globe.Shared.Services;
using Globe.Shared.Utilities;
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
        private readonly ILogService _logService;
        private readonly INotificationService _notificationService;
        private readonly IConceptManagementFiltersService _conceptManagementFiltersService;
        private readonly IConceptManagementViewService _conceptManagementViewService;  
        private readonly IVisibilityFiltersService _visibilityFiltersService;
        private readonly IDialogService _dialogService;

        public ConceptManagementWindowViewModel(
            IIdentityStore identityStore,
            IEventAggregator eventAggregator,
            ILogService logService,
            INotificationService notificationService,
            IConceptManagementFiltersService conceptManagementFiltersService,
            IConceptManagementViewService conceptManagementViewService,
            ICurrentJobConceptViewService currentJobConceptViewService,
            ILocalizationAppService localizationAppService,
            IVisibilityFiltersService visibilityFiltersService,
            IDialogService dialogService)
            : base(identityStore, eventAggregator, localizationAppService)
        {
            _logService = logService;
            _notificationService = notificationService;
            _conceptManagementFiltersService = conceptManagementFiltersService;
            _conceptManagementViewService = conceptManagementViewService;
            _visibilityFiltersService = visibilityFiltersService;
            _dialogService = dialogService;
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

        int _itemCount;
        public int ItemCount
        {
            get => _itemCount;
            set
            {
                SetProperty(ref _itemCount, value);
            }
        }

        string _insertedString = string.Empty;
        public string InsertedString
        {
            get => _insertedString;
            set 
            { 
                SetProperty(ref _insertedString, value); 
            }
        }

        string _insertedConcept = string.Empty;
        public string InsertedConcept
        {
            get => _insertedConcept;
            set
            {
                SetProperty(ref _insertedConcept, value);
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

        IEnumerable<TranslatedConcept> _translatedConcepts;
        public IEnumerable<TranslatedConcept> TranslatedConcepts
        {
            get => _translatedConcepts;
            set
            {
                SetProperty(ref _translatedConcepts, value);
            }
        }

        private DelegateCommand _exportToXmlCommand = null;
        public DelegateCommand ExportToXmlCommand =>
            _exportToXmlCommand ??= new DelegateCommand(() =>
            {
                _dialogService.ShowDialog(DialogNames.EXPORT_DB);          
            });

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

                this.InternalNamespaces = await _conceptManagementFiltersService.GetInternalNamespacesAsync(this.SelectedComponentNamespace != null ? this.SelectedComponentNamespace.Description : SharedConstants.COMPONENT_NAMESPACE_ALL);
                this.SelectedInternalNamespace = this.InternalNamespaces.FirstOrDefault();

                this.FiltersBusy = false;
            }));

        async protected override Task OnLoad(string fromView, object data)
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
                ShowFilters = _visibilityFiltersService.Visible;

                this.Contexts = await _conceptManagementFiltersService.GetContextAsync();
                this.ComponentNamespaces = await _conceptManagementFiltersService.GetComponentNamespacesAsync();
                this.InternalNamespaces = await _conceptManagementFiltersService.GetInternalNamespacesAsync(SelectedComponentNamespace != null ? SelectedComponentNamespace.Description : SharedConstants.COMPONENT_NAMESPACE_ALL);
                this.Languages = await _conceptManagementFiltersService.GetLanguagesAsync();

                this.SelectedContext = this.Contexts.FirstOrDefault();
                this.SelectedComponentNamespace = this.ComponentNamespaces.FirstOrDefault();
                this.SelectedInternalNamespace = this.InternalNamespaces.FirstOrDefault();
                this.SelectedLanguage = this.Languages.FirstOrDefault();
            }
            catch (Exception e)
            {
                _logService.Exception(e);
                await _notificationService.NotifyAsync(new Notification
                {
                    Title = Localize[LanguageKeys.Error],
                    Message = Localize[LanguageKeys.Error_during_filters_initialization],
                    Level = NotificationLevel.Error
                });

            }
            finally
            {
                this.FiltersBusy = false;
            }
        }

        private async Task OnSearch()
        {
            GridBusy = true;
            TranslatedConcepts = null;
            ItemCount = 0;

            try
            {
                if (
                    SelectedContext == null ||
                    SelectedComponentNamespace == null ||
                    SelectedInternalNamespace == null ||
                    SelectedLanguage == null)
                {
                    TranslatedConcepts = null;
                }
                else
                {
                    TranslatedConcepts = await _conceptManagementViewService.GetTranslatedConceptSAsync(
                        new ConceptManagementSearch
                        {
                            ComponentNamespace = SelectedComponentNamespace.Description,
                            InternalNamespace = SelectedInternalNamespace.Description,
                            LanguageId = SelectedLanguage.Id,
                            Context = SelectedContext.Name,
                            Concept = InsertedConcept,
                            LocalizedString = InsertedString
                        });

                    ItemCount = TranslatedConcepts.Count();
                }
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
            TranslatedConcepts = null;
            ItemCount = 0;
        }
    }
}
