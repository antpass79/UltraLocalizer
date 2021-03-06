﻿using MyLabLocalizer.Dialogs;
using MyLabLocalizer.Models;
using MyLabLocalizer.Services;
using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Shared.Services;
using MyLabLocalizer.Shared.Utilities;
using MyLabLocalizer.Core.Localization;
using MyLabLocalizer.Core.Services;
using MyLabLocalizer.Core.Services.Notifications;
using MyLabLocalizer.Core.Utilities;
using MyLabLocalizer.Core.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyLabLocalizer.ViewModels
{
    internal class JobListManagementWindowViewModel : LocalizeWindowViewModel
    {
        private readonly ILogService _logService;
        private readonly IDialogService _dialogService;
        private readonly IJobListManagementFiltersService _jobListManagementFiltersService;
        private readonly IJobListManagementService _jobListManagementService;
        private readonly INotificationService _notificationService;
        private readonly IVisibilityFiltersService _visibilityFiltersService;

        public JobListManagementWindowViewModel(
            IIdentityStore identityStore,
            IEventAggregator eventAggregator,
            ILogService logService,
            IDialogService dialogService,
            IJobListManagementFiltersService jobListManagementFiltersService,
            IJobListManagementService jobListManagementService,
            ILocalizationAppService localizationAppService,
            INotificationService notificationService,
            IVisibilityFiltersService visibilityFiltersService)
            : base(identityStore, eventAggregator, localizationAppService)
        {
            _logService = logService;
            _dialogService = dialogService;
            _jobListManagementFiltersService = jobListManagementFiltersService;
            _jobListManagementService = jobListManagementService;
            _notificationService = notificationService;
            _visibilityFiltersService = visibilityFiltersService;
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

        BindableNotTranslatedConceptView _selectedNotTranslatedConceptView;
        public BindableNotTranslatedConceptView SelectedNotTranslatedConceptView
        {
            get => _selectedNotTranslatedConceptView;
            set
            {
                SetProperty(ref _selectedNotTranslatedConceptView, value);
            }
        }

        IEnumerable<BindableComponentNamespaceGroup> _componentNamespaceGroups;
        public IEnumerable<BindableComponentNamespaceGroup> ComponentNamespaceGroups
        {
            get => _componentNamespaceGroups;
            set
            {
                SetProperty(ref _componentNamespaceGroups, value);
            }
        }

        IEnumerable<BindableNotTranslatedConceptView> _notTranslatedConceptViews;
        public IEnumerable<BindableNotTranslatedConceptView> NotTranslatedConceptViews
        {
            get => _notTranslatedConceptViews;
            set
            {
                SetProperty(ref _notTranslatedConceptViews, value);
            }
        }

        public BindableInternalNamespace SelectedInternalNamespace
        {
            get
            {
                if (ComponentNamespaceGroups == null)
                    return null;

                return ComponentNamespaceGroups
                    .SelectMany(item => item.InternalNamespaces)
                    .SingleOrDefault(item => item.IsSelected);
            }
        }

        public BindableComponentNamespaceGroup SelectedComponentNamespaceGroup
        {
            get
            {
                if (SelectedInternalNamespace == null)
                    return ComponentNamespaceGroups.SingleOrDefault(item => item.IsSelected == true);

                return ComponentNamespaceGroups
                    .SingleOrDefault(item => item.InternalNamespaces.Contains(SelectedInternalNamespace));
            }
        }

        readonly SmartBusy _smartConceptDetailsBusy = new SmartBusy();
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

        readonly SmartBusy _smartFiltersBusy = new SmartBusy();
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
            _languageChangeCommand ??= new DelegateCommand(async () =>
            {
                await OnLanguageChange();
                ItemCount = 0;
            });

        private DelegateCommand _addAllCommand = null;
        public DelegateCommand AddAllCommand =>
            _addAllCommand ??= new DelegateCommand(async () =>
            {
                try
                {
                    ConceptDetailsBusy = true;

                    using var taskRunner = new ParallelEnumerableResultTaskRunner<BindableNotTranslatedConceptView>();
                    foreach (var componentNamespaceGroup in ComponentNamespaceGroups)
                        foreach (var internalNamespace in componentNamespaceGroup.InternalNamespaces)
                            taskRunner.Add(_jobListManagementService.GetNotTranslatedConceptsAsync(componentNamespaceGroup.ComponentNamespace, internalNamespace, SelectedLanguage));
                    var result = await taskRunner.RunAsync();

                    UpdateNotTranslatedConceptViews(result);
                }
                catch (Exception e)
                {
                    _logService.Exception(e);
                    await _notificationService.NotifyAsync(new Notification
                    {
                        Title = Localize[LanguageKeys.Error],
                        Message = Localize[LanguageKeys.Error_during_getting_list],
                        Level = NotificationLevel.Error
                    });
                }
                finally
                {
                    ConceptDetailsBusy = false;
                    ItemCount = NotTranslatedConceptViews == null ? 0 : NotTranslatedConceptViews.Count();
                }
            });

        private DelegateCommand _removeAllCommand = null;
        public DelegateCommand RemoveAllCommand =>
            _removeAllCommand ??= new DelegateCommand(() =>
            {
                NotTranslatedConceptViews = null;
                ItemCount = 0;
            }, () => NotTranslatedConceptViews != null && NotTranslatedConceptViews.Any());

        private DelegateCommand<BindableComponentNamespaceGroup> _addComponentNamespaceGroupCommand = null;
        public DelegateCommand<BindableComponentNamespaceGroup> AddComponentNamespaceGroupCommand =>
            _addComponentNamespaceGroupCommand ??= new DelegateCommand<BindableComponentNamespaceGroup>(async (componentNamespaceGroup) =>
            {
                try
                {
                    ConceptDetailsBusy = true;

                    using var taskRunner = new ParallelEnumerableResultTaskRunner<BindableNotTranslatedConceptView>();
                    foreach (var internalNamespace in componentNamespaceGroup.InternalNamespaces)
                        taskRunner.Add(_jobListManagementService.GetNotTranslatedConceptsAsync(componentNamespaceGroup.ComponentNamespace, internalNamespace, SelectedLanguage));
                    var result = await taskRunner.RunAsync();

                    UpdateNotTranslatedConceptViews(result);
                }
                catch (Exception e)
                {
                    _logService.Exception(e);
                    await _notificationService.NotifyAsync(new Notification
                    {
                        Title = Localize[LanguageKeys.Error],
                        Message = Localize[LanguageKeys.Error_during_getting_list],
                        Level = NotificationLevel.Error
                    });
                }
                finally
                {
                    ConceptDetailsBusy = false;
                    ItemCount = NotTranslatedConceptViews == null ? 0 : NotTranslatedConceptViews.Count();
                }
            });

        private DelegateCommand<BindableInternalNamespace> _addInternalNamespaceCommand = null;
        public DelegateCommand<BindableInternalNamespace> AddInternalNamespaceCommand =>
            _addInternalNamespaceCommand ??= new DelegateCommand<BindableInternalNamespace>(async (internalNamespace) =>
            {
                try
                {
                    ConceptDetailsBusy = true;

                    using var taskRunner = new ParallelEnumerableResultTaskRunner<BindableNotTranslatedConceptView>();
                    taskRunner.Add(_jobListManagementService.GetNotTranslatedConceptsAsync(SelectedComponentNamespaceGroup.ComponentNamespace, internalNamespace, SelectedLanguage));
                    var result = await taskRunner.RunAsync();
                    
                    UpdateNotTranslatedConceptViews(result);
                }
                catch (Exception e)
                {
                    _logService.Exception(e);
                    await _notificationService.NotifyAsync(new Notification
                    {
                        Title = Localize[LanguageKeys.Error],
                        Message = Localize[LanguageKeys.Error_during_getting_list],
                        Level = NotificationLevel.Error
                    });
                }
                finally
                {
                    ConceptDetailsBusy = false;
                    ItemCount = NotTranslatedConceptViews == null ? 0 : NotTranslatedConceptViews.Count();
                }
            });

        private DelegateCommand _removeCommand = null;
        public DelegateCommand RemoveCommand =>
            _removeCommand ??= new DelegateCommand(() =>
            {
                NotTranslatedConceptViews = NotTranslatedConceptViews?.Where(item => !item.IsSelected);
                ItemCount = NotTranslatedConceptViews == null ? 0 : NotTranslatedConceptViews.Count();
            }, () => SelectedNotTranslatedConceptView != null);

        private DelegateCommand _saveJoblistCommand = null;
        public DelegateCommand SaveJoblistCommand =>
            _saveJoblistCommand ??= new DelegateCommand(() =>
            {
                var @params = new Prism.Services.Dialogs.DialogParameters
                {
                    { Dialogs.DialogParams.LANGUAGE, SelectedLanguage },
                    { Dialogs.DialogParams.NOT_TRANSLATED_CONCEPT_VIEWS, NotTranslatedConceptViews }
                };

                _dialogService.ShowDialog(DialogNames.SAVE_JOBLIST, @params, dialogResult =>
                {
                    if (dialogResult.Result == ButtonResult.OK)
                    {
                        NotTranslatedConceptViews = null;
                        ItemCount = 0;
                    }
                });
            }, () => NotTranslatedConceptViews != null && NotTranslatedConceptViews.Any());

        async protected override Task OnLoad(string fromView, object data)
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

        private void UpdateNotTranslatedConceptViews(IEnumerable<BindableNotTranslatedConceptView> notTranslatedConceptViews)
        {
            var result = notTranslatedConceptViews
                .OrderBy(item => item.ComponentNamespace.Description)
                .ThenBy(item => item.InternalNamespace.Description)
                .ThenBy(item => item.Name)
                .ToList();

            if (NotTranslatedConceptViews == null)
            {
                NotTranslatedConceptViews = result;
            }
            else
            {
                var newElements = result
                    .Where(item => !NotTranslatedConceptViews.Any(gridItem => gridItem.Name == item.Name))
                    .ToList();

                NotTranslatedConceptViews = NotTranslatedConceptViews.Union(newElements);
            }
        }

        async private Task InitializeFilters()
        {
            FiltersBusy = true;

            try
            {
                ShowFilters = _visibilityFiltersService.Visible;
                Languages = await _jobListManagementFiltersService.GetLanguagesAsync();
                SelectedLanguage = Languages.FirstOrDefault(item => item.IsoCoding == SharedConstants.LANGUAGE_EN);
            }
            catch (Exception e)
            {
                _logService.Exception(e);
                await _notificationService.NotifyAsync(new Notification
                {
                    Title = Localize[LanguageKeys.Error],
                    Message = Localize[LanguageKeys.Error_during_searching_new_concepts],
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

                ComponentNamespaceGroups = await _jobListManagementService.GetComponentNamespaceGroupsAsync(SelectedLanguage);
                ComponentsVisible = ComponentNamespaceGroups != null && ComponentNamespaceGroups.Any();
                NotTranslatedConceptViews = null;
            }
            catch (Exception e)
            {
                _logService.Exception(e);

                NotTranslatedConceptViews = null;
                ComponentNamespaceGroups = null;
                await _notificationService.NotifyAsync(new Notification
                {
                    Title = Localize[LanguageKeys.Error],
                    Message = Localize[LanguageKeys.Error_during_building_groups],
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
            ComponentNamespaceGroups = null;
            ItemCount = 0;
        }
    }
}
