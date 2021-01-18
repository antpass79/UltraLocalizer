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
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.ViewModels
{
    internal class CurrentJobWindowViewModel : LocalizeWindowViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly ILogService _logService;
        private readonly INotificationService _notificationService;
        private readonly ICurrentJobFiltersService _currentJobFiltersService;
        private readonly ICurrentJobConceptViewService _currentJobConceptViewService;
        private readonly IXmlService _xmlService;

        public CurrentJobWindowViewModel(
            IEventAggregator eventAggregator,
            IDialogService dialogService,
            ILogService logService,
            INotificationService notificationService,
            ICurrentJobFiltersService currentJobFiltersService,
            ICurrentJobConceptViewService currentJobConceptViewService,
            ILocalizationAppService localizationAppService,
            IXmlService xmlService)
            : base(eventAggregator, localizationAppService)
        {
            _dialogService = dialogService;
            _logService = logService;
            _notificationService = notificationService;
            _currentJobFiltersService = currentJobFiltersService;
            _currentJobConceptViewService = currentJobConceptViewService;
            _xmlService = xmlService;
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

        IEnumerable<Globe.Client.Localizer.Models.BindableInternalNamespace> _internalNamespaces;
        public IEnumerable<Globe.Client.Localizer.Models.BindableInternalNamespace> InternalNamespaces
        {
            get => _internalNamespaces;
            set
            {
                SetProperty(ref _internalNamespaces, value);
            }
        }

        Globe.Client.Localizer.Models.BindableInternalNamespace _selectedInternalNamespace;
        public Globe.Client.Localizer.Models.BindableInternalNamespace SelectedInternalNamespace
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
            _searchCommand ?? (_searchCommand = new DelegateCommand(async () =>
            {
                await OnSearch();
            }));

        private DelegateCommand<JobListConcept> _conceptViewEditCommand = null;
        public DelegateCommand<JobListConcept> ConceptViewEditCommand =>
            _conceptViewEditCommand ?? (_conceptViewEditCommand = new DelegateCommand<JobListConcept>(async (conceptView) =>
            {
                ConceptDetails conceptDetails = new ConceptDetails();

                ConceptDetailsBusy = true;

                try
                {
                    conceptDetails = await _currentJobConceptViewService.GetConceptDetailsAsync(conceptView);
                }
                catch (Exception e)
                {
                    _logService.Exception(e);
                }
                finally
                {
                    ConceptDetailsBusy = false;
                }

                var @params = new DialogParameters();

                @params.Add(DialogParams.EDITABLE_CONCEPT, new EditableConcept(
                    conceptView.Id,
                    conceptView.ComponentNamespace,
                    conceptView.InternalNamespace,
                    conceptView.Name,
                    conceptDetails.SoftwareDeveloperComment,
                    new ObservableCollection<EditableContext>(conceptView.ContextViews.Select(contextView => new EditableContext(conceptDetails.OriginalStringContextValues
                        .Single(item => item.ContextName == contextView.Name).StringValue, contextView.StringValue, contextView.StringId)
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
                @params.Add(DialogParams.LANGUAGE, this.SelectedLanguage);

                _dialogService.ShowDialog(DialogNames.STRING_EDITOR, @params, async dialogResult =>
                { 
                    if(dialogResult.Result == ButtonResult.OK)
                        await OnSearch();
                });
            }));

        private DelegateCommand _exportToXmlCommand = null;
        public DelegateCommand ExportToXmlCommand =>
            _exportToXmlCommand ?? (_exportToXmlCommand = new DelegateCommand(async () =>
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
            }));

        private DelegateCommand _componentNamespaceChangeCommand = null;
        public DelegateCommand ComponentNamespaceChangeCommand =>
            _componentNamespaceChangeCommand ?? (_componentNamespaceChangeCommand = new DelegateCommand(async () =>
            {
                this.FiltersBusy = true;

                this.InternalNamespaces = await _currentJobFiltersService.GetInternalNamespacesAsync(this.SelectedComponentNamespace != null ? this.SelectedComponentNamespace.Description : SharedConstants.COMPONENT_NAMESPACE_ALL);
                this.SelectedInternalNamespace = this.InternalNamespaces.FirstOrDefault();

                this.FiltersBusy = false;
            }));

        private DelegateCommand _languageChangeCommand = null;
        public DelegateCommand LanguageChangeCommand =>
            _languageChangeCommand ?? (_languageChangeCommand = new DelegateCommand(async () =>
            {
                this.FiltersBusy = true;

                this.JobItems = await _currentJobFiltersService.GetJobItemsAsync(this.Identity.Name, this.SelectedLanguage != null ? this.SelectedLanguage.IsoCoding : SharedConstants.LANGUAGE_ALL);
                this.SelectedJobItem = this.JobItems.FirstOrDefault();

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
                this.JobItems = await _currentJobFiltersService.GetJobItemsAsync(this.Identity.Name, this.SelectedLanguage != null ? this.SelectedLanguage.IsoCoding : SharedConstants.LANGUAGE_ALL);
                this.ComponentNamespaces = await _currentJobFiltersService.GetComponentNamespacesAsync();
                this.InternalNamespaces = await _currentJobFiltersService.GetInternalNamespacesAsync(this.SelectedComponentNamespace != null ? this.SelectedComponentNamespace.Description : SharedConstants.COMPONENT_NAMESPACE_ALL);
                this.Languages = await _currentJobFiltersService.GetLanguagesAsync();

                this.SelectedJobItem = this.JobItems.FirstOrDefault();
                this.SelectedComponentNamespace = this.ComponentNamespaces.FirstOrDefault();
                this.SelectedInternalNamespace = this.InternalNamespaces.FirstOrDefault();
                this.SelectedLanguage = this.Languages.FirstOrDefault(item => item.IsoCoding == SharedConstants.LANGUAGE_EN);
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

        private async Task OnSearch()
        {
            GridBusy = true;
            ConceptViews = null;
            ItemCount = 0;

            try
            {
                if (
                    SelectedJobItem == null ||
                    SelectedComponentNamespace == null ||
                    SelectedInternalNamespace == null ||
                    SelectedLanguage == null)
                {
                    ConceptViews = null;
                }
                else
                {
                    ConceptViews = await _currentJobConceptViewService.GetConceptViewsAsync(
                        new JobListConceptSearch
                        {
                            ComponentNamespace = SelectedComponentNamespace.Description,
                            InternalNamespace = SelectedInternalNamespace.Description,
                            LanguageId = SelectedLanguage.Id,
                            JobListId = SelectedJobItem.Id
                        });

                    ItemCount = ConceptViews.Count();
                }
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

        private string ChooseFilePathToDownloadXml()
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.FileName = "xml";
            saveDialog.DefaultExt = ".zip";
            saveDialog.Filter = "Zip documents (.zip)|*.zip";

            if (saveDialog.ShowDialog() != true)
                return null;

            return saveDialog.FileName;
        }
    }
}
