﻿using MyLabLocalizer.Models;
using MyLabLocalizer.Services;
using Globe.Client.Platofrm.Events;
using MyLabLocalizer.Core.Services;
using MyLabLocalizer.Core.Services.Notifications;
using MyLabLocalizer.Core.ViewModels;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace MyLabLocalizer.ViewModels
{
    internal class MergeWindowViewModel : AuthorizeWindowViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IAsyncLocalizableStringService _fileSystemLocalizableStringService;
        private readonly IAsyncLocalizableStringService _httpLocalizableStringService;
        private readonly IStringMergeService _stringMergeService;
        private readonly ILocalizationAppService _localizationAppService;
        private readonly INotificationService _notificationService;

        public MergeWindowViewModel(
            IIdentityStore identityStore,
            IEventAggregator eventAggregator,
            IFileSystemLocalizableStringService fileSystemLocalizableStringService,
            IHttpLocalizableStringService httpLocalizableStringService,
            IStringMergeService stringMergeService,
            ILocalizationAppService localizationAppService,
            INotificationService notificationService)
            : base(identityStore, eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _fileSystemLocalizableStringService = fileSystemLocalizableStringService;
            _httpLocalizableStringService = httpLocalizableStringService;
            _stringMergeService = stringMergeService;
            _localizationAppService = localizationAppService;
            _notificationService = notificationService;
        }

        IEnumerable<LocalizableString> _fileSystemStrings;
        public IEnumerable<LocalizableString> FileSystemStrings
        {
            get => _fileSystemStrings;
            set
            {
                SetProperty<IEnumerable<LocalizableString>>(ref _fileSystemStrings, value);
            }
        }

        IEnumerable<LocalizableString> _httpStrings;
        public IEnumerable<LocalizableString> HttpStrings
        {
            get => _httpStrings;
            set
            {
                SetProperty<IEnumerable<LocalizableString>>(ref _httpStrings, value);
            }
        }

        IEnumerable<LocalizableString> _mergedStrings;
        public IEnumerable<LocalizableString> MergedStrings
        {
            get => _mergedStrings;
            set
            {
                SetProperty<IEnumerable<LocalizableString>>(ref _mergedStrings, value);
            }
        }

        private DelegateCommand _loadCommand = null;
        public DelegateCommand LoadCommand =>
            _loadCommand ?? (_loadCommand = new DelegateCommand(async () =>
            {
                this.FileSystemStrings = await _fileSystemLocalizableStringService.GetAllAsync();
                this.HttpStrings = await _httpLocalizableStringService.GetAllAsync();
                AutoMergeCommand.RaiseCanExecuteChanged();
            }));

        private DelegateCommand _autoMergeCommand = null;
        public DelegateCommand AutoMergeCommand =>
            _autoMergeCommand ?? (_autoMergeCommand = new DelegateCommand(() =>
            {
                this.MergedStrings = _stringMergeService.Merge(this.FileSystemStrings, this.HttpStrings);
                SaveCommand.RaiseCanExecuteChanged();
            },
            () =>
            {
                return this.FileSystemStrings != null && this.HttpStrings != null && (this.FileSystemStrings.Count() > 0 || this.HttpStrings.Count() > 0);
            }));

        private DelegateCommand _saveCommand = null;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(async () =>
            {
                _eventAggregator.GetEvent<BusyChangedEvent>().Publish(true);

                try
                {
                    await _httpLocalizableStringService.SaveAsync(this.MergedStrings);
                    await _notificationService.NotifyAsync(new Notification
                    {
                        Title = string.Empty,
                        Message = _localizationAppService.Resolve("Operation_successfully_completed"),
                        Level = NotificationLevel.Info
                    });
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    await _notificationService.NotifyAsync(new Notification
                    {
                        Title = string.Empty,
                        Message = _localizationAppService.Resolve("Error_during_server_communication"),
                        Level = NotificationLevel.Error
                    });
                }
                finally
                {
                    _eventAggregator.GetEvent<BusyChangedEvent>().Publish(false);
                }
            },
            () =>
            {
                return this.MergedStrings != null && this.MergedStrings.Count() > 0;
            }));

        private DelegateCommand<LocalizableString> _selectFromFileSystemCommand = null;
        public DelegateCommand<LocalizableString> SelectFromFileSystemCommand =>
            _selectFromFileSystemCommand ?? (_selectFromFileSystemCommand = new DelegateCommand<LocalizableString>((localizableString) =>
            {
                if (MergedStrings == null)
                    return;

                foreach (var item in MergedStrings)
                {
                    if (localizableString.Key == item.Key)
                    {
                        item.SavedOn = localizableString.SavedOn;
                        item.Value = localizableString.Value;

                        break;
                    }
                }

                MergedStrings = MergedStrings.ToList();
            }));
        
        private DelegateCommand<LocalizableString> _selectFromServerCommand = null;
        public DelegateCommand<LocalizableString> SelectFromServerCommand =>
            _selectFromServerCommand ?? (_selectFromServerCommand = new DelegateCommand<LocalizableString>((localizableString) =>
            {
                if (MergedStrings == null)
                    return;

                foreach (var item in MergedStrings)
                {
                    if (localizableString.Key == item.Key)
                    {
                        item.SavedOn = localizableString.SavedOn;
                        item.Value = localizableString.Value;

                        break;
                    }
                }

                MergedStrings = MergedStrings.ToList();
            }));

        protected override void OnAuthenticationChanged(IPrincipal principal)
        {
            base.OnAuthenticationChanged(principal);

            this.FileSystemStrings = new List<LocalizableString>();
            this.HttpStrings = new List<LocalizableString>();
            this.MergedStrings = new List<LocalizableString>();
            AutoMergeCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}
