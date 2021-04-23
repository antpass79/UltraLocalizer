﻿using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Assets.Localization;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.ViewModels;
using Globe.Client.Platofrm.Events;
using Globe.Shared.DTOs;
using Globe.Shared.Services;
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
        private readonly IEventAggregator _eventAggregator;
        private readonly INotificationService _notificationService;
        private readonly IVisibilityFiltersService _visibilityFiltersService;
        private readonly IXmlService _xmlService;

        public ExportDbViewModel(
            ILogService logService,
            IEventAggregator eventAggregator,
            IIdentityStore identityStore,
            ILocalizationAppService localizationAppService,
            INotificationService notificationService,
            IVisibilityFiltersService visibilityFiltersService,
            IXmlService xmlService)
            : base(identityStore, eventAggregator, localizationAppService)
        {
            _logService = logService;
            _eventAggregator = eventAggregator;
            _notificationService = notificationService;
            _visibilityFiltersService = visibilityFiltersService;
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

        IEnumerable<BindableComponentNamespaceGroup> _componentNamespaceGroups;
        public IEnumerable<BindableComponentNamespaceGroup> ComponentNamespaceGroups
        {
            get => _componentNamespaceGroups;
            set
            {
                SetProperty(ref _componentNamespaceGroups, value);
            }
        }

        public IEnumerable<BindableComponentNamespaceGroup> SelectedComponentNamespaceGroups
        {
            //Check. TODO
            get
            {
                if (SelectedInternalNamespace == null)
                    return ComponentNamespaceGroups.Where(item => item.IsSelected);

                return ComponentNamespaceGroups
                    .Where(item => item.InternalNamespaces.Contains(SelectedInternalNamespace));
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

                //TODO.
                //Get All Data -> Languages, component, internalNamespaces
            }
            catch (Exception e)
            {
                _logService.Exception(e);
                //Put all data = null
            }
            finally
            {
                Busy = false;
            }
        }

        //TODO probabilmente non c'e' bisogno di alzare manualmente onProperty change. Indagare
        //protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        //{
        //    base.OnPropertyChanged(args);

        //    if (args.PropertyName == nameof(JobListName))
        //    {
        //        ExportToXmlCommand.RaiseCanExecuteChanged();           
        //    }
        //}

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