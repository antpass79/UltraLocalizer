using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Assets.Localization;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.Services.Notifications;
using Globe.Client.Platform.ViewModels;
using Globe.Client.Platofrm.Events;
using Globe.Shared.DTOs;
using Globe.Shared.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Globe.Client.Localizer.Dialogs.ViewModels
{
    class ExportDbViewModel : LocalizeWindowViewModel, IDialogAware
    {
        #region Data Members

        private readonly ILogService _logService;
        private readonly INotificationService _notificationService;
        private readonly IExportDbFiltersService _exportDbFiltersService;
        private readonly IXmlService _xmlService;

        #endregion

        #region Constructors

        public ExportDbViewModel(
            ILogService logService,
            IEventAggregator eventAggregator,
            IIdentityStore identityStore,
            ILocalizationAppService localizationAppService,
            INotificationService notificationService,
            IExportDbFiltersService exportDbFiltersService,
            IXmlService xmlService)
            : base(identityStore, eventAggregator, localizationAppService)
        {
            _logService = logService;
            _notificationService = notificationService;
            _exportDbFiltersService = exportDbFiltersService;
            _xmlService = xmlService;
        }

        #endregion

        #region Properties

        private string _title = DialogNames.EXPORT_DB;
        public string Title
        {
            get { return _title; }
            private set { SetProperty(ref _title, value); }
        }

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            private set { SetProperty(ref _busy, value); }
        }

        bool _showFilters = false;
        public bool ShowFilters
        {
            get => _showFilters;
            set { SetProperty(ref _showFilters, value); }
        }

        bool _isExportModeFull= true;
        public bool IsExportModeFull
        {
            get => _isExportModeFull;
            set { SetProperty(ref _isExportModeFull, value); }
        }

        bool _isSelectAllLanguagesChecked = false;
        public bool IsSelectAllLanguagesChecked
        {
            get => _isSelectAllLanguagesChecked;
            set { SetProperty(ref _isSelectAllLanguagesChecked, value); }
        }

        bool _isSelectAllComponentsChecked = false;
        public bool IsSelectAllComponentsChecked
        {
            get => _isSelectAllComponentsChecked;
            set { SetProperty(ref _isSelectAllComponentsChecked, value); }
        }

        IEnumerable<BindableLanguage> _languages;
        public IEnumerable<BindableLanguage> Languages
        {
            get => _languages;
            set
            {
                SetProperty(ref _languages, value);
            }
        }

        public IEnumerable<BindableLanguage> CheckedLanguages
        {
            get
            {
                if (Languages == null)
                    return null;

                return Languages.Where(item => item.IsSelected);
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

        public IEnumerable<BindableComponentNamespaceGroup> CheckedComponentNamespaceGroups
        {
            get
            {
                if (ComponentNamespaceGroups == null)
                    return null;

                return ComponentNamespaceGroups.Where(item => !item.IsSelected.HasValue && item.IsSelected == true);

            }
        }

        private ExportDbFilters _exportDbFilters;
        public ExportDbFilters ExportDbFilters
        {
            get { return _exportDbFilters; }
            set { SetProperty(ref _exportDbFilters, value); }
        }

        #endregion

        #region Commands

        private DelegateCommand _exportToXmlCommand = null;
        public DelegateCommand ExportToXmlCommand =>
            _exportToXmlCommand ??= new DelegateCommand(async () =>
            {
                if(!IsExportModeFull)
                {
                    ExportDbFilters = new ExportDbFilters
                    {
                        Languages = CheckedLanguages.Select( bindablelanguage => new Language
                        {
                            Name = bindablelanguage.Name,
                            Description = bindablelanguage.Description,
                            Id = bindablelanguage.Id,
                            IsoCoding = bindablelanguage.IsoCoding
                        }),
                        ComponentNamespaceGroups = CheckedComponentNamespaceGroups.Select(componentNamespace => new ComponentNamespaceGroup<ComponentNamespace, InternalNamespace>
                        {
                            ComponentNamespace = new ComponentNamespace { Description = componentNamespace.ComponentNamespace.Description },
                            InternalNamespaces = componentNamespace.InternalNamespaces
                            .Where(internalNamespaceGrouped => internalNamespaceGrouped.IsSelected)
                            .Select(internalNamespace => new InternalNamespace
                            {
                                Description = internalNamespace.Description
                            })
                        })
                    };
                }

                var downloadPath = ChooseFilePathToDownloadXml();
                if (string.IsNullOrWhiteSpace(downloadPath))
                    return;

                EventAggregator
                .GetEvent<BackgroundBusyChangedEvent>()
                .Publish(true);

                RaiseRequestClose(new DialogResult(ButtonResult.OK));

                try
                {
                    await _xmlService.Download(ExportDbFilters, downloadPath);
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
               
            }, () => CanExport());

        private DelegateCommand _closeDialogCommand;
        public DelegateCommand CloseDialogCommand =>
            _closeDialogCommand ??= new DelegateCommand(() =>
            {
                RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
            });

        private DelegateCommand _selectAllComponentsCommand;
        public DelegateCommand SelectAllComponentsCommand =>
            _selectAllComponentsCommand ??= new DelegateCommand(() =>
           {
               foreach (BindableComponentNamespaceGroup bindableComponentNamespaceGroup in ComponentNamespaceGroups)
               {
                   bindableComponentNamespaceGroup.IsSelected = IsSelectAllComponentsChecked;
               }
           });

        private DelegateCommand _selectAllLanguagesCommand;
        public DelegateCommand SelectAllLanguagesCommand =>
            _selectAllLanguagesCommand ??= new DelegateCommand(() =>
            {
                foreach (BindableLanguage bindableLanguage in Languages)
                {
                    bindableLanguage.IsSelected = IsSelectAllLanguagesChecked;
                }
            });

        #endregion

        #region IDialogAware Interface

        public event Action<IDialogResult> RequestClose;
        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
        }

        async public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                Busy = true;
                ComponentNamespaceGroups = new ObservableCollection<BindableComponentNamespaceGroup>(await _exportDbFiltersService.GetAllComponentNamespaceGroupsAsync());
                Languages = await _exportDbFiltersService.GetLanguagesAsync();
            }
            catch (Exception e)
            {
                _logService.Exception(e);              
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
                Busy = false;
            }
        }

        #endregion

        #region Protected functions

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(IsExportModeFull) || args.PropertyName == nameof(ExportDbFilters))
            {
                ExportToXmlCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Private Functions

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

        private bool CanExport()//TODO
        {
            if (IsExportModeFull || ExportDbFilters == null)
                return true;
            else
                return (ExportDbFilters.Languages.Count() > 0 && ExportDbFilters.ComponentNamespaceGroups.Count() > 0);
        }

        #endregion
    }
}
