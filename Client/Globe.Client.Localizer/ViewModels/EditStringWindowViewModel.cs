﻿using Globe.Client.Localizer.Dialogs;
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
    internal class EditStringWindowViewModel : LocalizeWindowViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly ILogService _logService;
        private readonly INotificationService _notificationService;
        private readonly IEditStringFiltersService _editStringFiltersService;
        private readonly IEditStringViewService _editStringViewService;
        private readonly IVisibilityFiltersService _visibilityFiltersService;

        public EditStringWindowViewModel(
            IIdentityStore identityStore,
            IEventAggregator eventAggregator,
            IDialogService dialogService,
            ILogService logService,
            INotificationService notificationService,
            IEditStringFiltersService editStringFiltersService,
            IEditStringViewService editStringViewService,
            ILocalizationAppService localizationAppService,
            IVisibilityFiltersService visibilityFiltersService)
            : base(identityStore, eventAggregator, localizationAppService)
        {
            _dialogService = dialogService;
            _logService = logService;
            _notificationService = notificationService;
            _editStringFiltersService = editStringFiltersService;
            _editStringViewService = editStringViewService;
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

        int _insertedStringId;
        public int InsertedStringId
        {
            get => _insertedStringId;
            set
            {
                SetProperty(ref _insertedStringId, value);
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

        readonly UsedFiltersBySearching _usedFiltersBySearching = new UsedFiltersBySearching();
        string _filterBy;
        public string FilterBy
        {
            get => _filterBy;
            private set
            {
                SetProperty(ref _filterBy, value);
            }
        }

        IEnumerable<LocalizeString> _localizeStrings;
        public IEnumerable<LocalizeString> LocalizeStrings
        {
            get => _localizeStrings;
            set
            {
                SetProperty(ref _localizeStrings, value);
            }
        }

        LocalizeString _selectedTranslatedConcept;
        public LocalizeString SelectedTranslatedConcept
        {
            get => _selectedTranslatedConcept;
            set
            {
                SetProperty(ref _selectedTranslatedConcept, value);
            }
        }

        private DelegateCommand _searchCommand = null;
        public DelegateCommand SearchCommand =>
            _searchCommand ??= new DelegateCommand(async () =>
            {
                UpdateFiltersUsedBySearching();
                await OnSearch();
            });

        private DelegateCommand<LocalizeString> _localizeStringViewEditCommand = null;
        public DelegateCommand<LocalizeString> LocalizeStringViewEditCommand =>
            _localizeStringViewEditCommand ??= new DelegateCommand<LocalizeString>(async (localizeStringView) =>
            {
                ConceptDetailsBusy = true;

                try
                {

                    var @params = new DialogParameters
                    {
                        {
                            //TODO: parametri vuoti
                            DialogParams.EDITABLE_CONCEPT,
                            new EditableConcept(
                                localizeStringView.StringId,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                new ObservableCollection<EditableContext>(localizeStringView.LocalizeStringDetails
                                .Select(details => new EditableContext(string.Empty,string.Empty, localizeStringView.Value, localizeStringView.StringId)
                        {
                            ComponentNamespace = details.ComponentNameSpace,
                            InternalNamespace = details.InternalNameSpace,
                            Concept = details.Concept,
                            Name = details.ContextName,
                            Concept2ContextId = details.Concept2ContextId,
                            StringType = !string.IsNullOrWhiteSpace(localizeStringView.StringType) ? Enum.Parse<StringType>(localizeStringView.StringType) : StringType.Label,
                            StringId = localizeStringView.StringId,
                        }).ToList()))
                            {
                                MasterTranslatorComment = string.Empty,//conceptDetails.MasterTranslatorComment,
                                IgnoreTranslation = false//details.Ignore
                            }
                        },
                        { DialogParams.LANGUAGE, SelectedLanguage }
                    };

                    _dialogService.ShowDialog(DialogNames.EDIT_TRANSLATED_STRING, @params, async dialogResult =>
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
                this.Languages = await _editStringFiltersService.GetLanguagesAsync();

                this.SelectedLanguage = this.Languages.FirstOrDefault(item => item.IsoCoding == SharedConstants.LANGUAGE_EN);

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
            LocalizeStrings = null;
            ItemCount = 0;

            try
            {
                LocalizeStrings = await _editStringViewService.GetTranslatedConceptsAsync(
                new EditStringSearch
                {
                    LanguageId = _usedFiltersBySearching.Language.Id,
                    StringId = InsertedStringId,
                    Concept = InsertedConcept,
                    LocalizedString = InsertedString
                });

                ItemCount = LocalizeStrings.Count();

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
            LocalizeStrings = null;
            ItemCount = 0;
        }

        private void UpdateFiltersUsedBySearching()
        {
            _usedFiltersBySearching.Language = SelectedLanguage;

            FilterBy = $"{Localize["FilterBy"]} {_usedFiltersBySearching?.Language?.Name}";//TODO: Guarda se aggiungere qualche altra info
        }
    }
}
