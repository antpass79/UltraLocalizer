using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Assets.Localization;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.Services.Notifications;
using Globe.Client.Platform.Utilities;
using Globe.Client.Platform.ViewModels;
using Globe.Client.Platofrm.Events;
using Globe.Shared.DTOs;
using Globe.Shared.Services;
using Globe.Shared.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Globe.Client.Localizer.Dialogs.ViewModels
{
    class ExportDbViewModel : LocalizeWindowViewModel, IDialogAware
    {
        private readonly ILogService _logService;
        private readonly INotificationService _notificationService;
        private readonly IExportDbFiltersService _exportDbFiltersService;
        private readonly IXmlService _xmlService;

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

        bool _isEnglishChecked = false;
        public bool IsEnglishChecked
        {
            get => _isEnglishChecked;
            set { SetProperty(ref _isEnglishChecked, value); }
        }

        bool _isFrenchChecked = false;
        public bool IsFrenchChecked
        {
            get => _isFrenchChecked;
            set { SetProperty(ref _isFrenchChecked, value); }
        }

        bool _isItalianChecked = false;
        public bool IsItalianChecked
        {
            get => _isItalianChecked;
            set { SetProperty(ref _isItalianChecked, value); }
        }

        bool _isGermanChecked = false;
        public bool IsGermanChecked
        {
            get => _isGermanChecked;
            set { SetProperty(ref _isGermanChecked, value); }
        }

        bool _isSpanishChecked = false;
        public bool IsSpanishChecked
        {
            get => _isSpanishChecked;
            set { SetProperty(ref _isSpanishChecked, value); }
        }

        bool _isChineseChecked = false;
        public bool IsChineseChecked
        {
            get => _isChineseChecked;
            set { SetProperty(ref _isChineseChecked, value); }
        }

        bool _isRussianChecked = false;
        public bool IsRussianChecked
        {
            get => _isRussianChecked;
            set { SetProperty(ref _isRussianChecked, value); }
        }

        bool _isPortugueseChecked = false;
        public bool IsPortugueseChecked
        {
            get => _isPortugueseChecked;
            set { SetProperty(ref _isPortugueseChecked, value); }
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
        //Inserire vincolo che quando si seleziona la root, tutti i figli vengono selezionati
        public IEnumerable<BindableComponentNamespaceGroup> CheckedComponentNamespaceGroups
        {
            get
            {
                return ComponentNamespaceGroups.Where(item => item.IsSelected);

                //return ComponentNamespaceGroups
                //    .Where(item => item.InternalNamespaces.Any(internalNamespace => internalNamespace.IsSelected));
            }
        }

        //public IEnumerable<BindableInternalNamespace> CheckedInternalNamespaces
        //{
        //    get
        //    {
        //        if (ComponentNamespaceGroups == null)
        //            return null;

        //        return ComponentNamespaceGroups
        //            .SelectMany(item => item.InternalNamespaces)
        //            .Where(item => item.IsSelected);
               
        //    }
        //}

        private ExportDbFilters _exportDbFilters;
        public ExportDbFilters ExportDbFilters
        {
            get { return _exportDbFilters; }
            set { SetProperty(ref _exportDbFilters, value); }
        }

        private DelegateCommand<ExportDbFilters> _exportToXmlCommand = null;
        public DelegateCommand<ExportDbFilters> ExportToXmlCommand =>
            _exportToXmlCommand ??= new DelegateCommand<ExportDbFilters>(async (exportDbFilters) =>
            {
                //In caso di esportazione Custom, creo oggetto da spedire al server con i filtri da applicare, altrimenti di default abbiamo esportazione Full
                if(!IsExportModeFull)
                {
                    exportDbFilters = new ExportDbFilters
                    {
                        ExportDbMode = ExportDbMode.Custom,
                        IsoCodeLanguages = GetSelectedLanguages(),
                        ComponentNamespaces = CheckedComponentNamespaceGroups.Select(item => item.ComponentNamespace.Description).ToList()
                        //InternalNamespaces = CheckedInternalNamespaces.Select(item => item.Description).ToList()
                    };
                }

                var downloadPath = ChooseFilePathToDownloadXml();
                if (string.IsNullOrWhiteSpace(downloadPath))
                    return;

                EventAggregator
                .GetEvent<BackgroundBusyChangedEvent>()
                .Publish(true);

                try
                {
                    await _xmlService.Download(exportDbFilters, downloadPath);
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
            });

        private DelegateCommand _closeDialogCommand;
        public DelegateCommand CloseDialogCommand =>
            _closeDialogCommand ??= new DelegateCommand(() =>
            {
                RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
            });

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
                ComponentNamespaceGroups = await _exportDbFiltersService.GetAllComponentNamespaceGroupsAsync();            
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

        private List<string> GetSelectedLanguages()
        {
            var languages = new List<string>();

            if (IsEnglishChecked)
                languages.Add(SharedConstants.LANGUAGE_EN);
            if (IsFrenchChecked)
                languages.Add(SharedConstants.LANGUAGE_FR);
            if (IsItalianChecked)
                languages.Add(SharedConstants.LANGUAGE_IT);
            if (IsGermanChecked)
                languages.Add(SharedConstants.LANGUAGE_DE);
            if (IsSpanishChecked)
                languages.Add(SharedConstants.LANGUAGE_ES);
            if (IsChineseChecked)
                languages.Add(SharedConstants.LANGUAGE_ZH);
            if (IsRussianChecked)
                languages.Add(SharedConstants.LANGUAGE_RU);
            if (IsPortugueseChecked)
                languages.Add(SharedConstants.LANGUAGE_PT);

            return languages;
        }

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
    }
}
