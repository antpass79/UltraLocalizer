﻿using MyLabLocalizer.Models;
using MyLabLocalizer.Services;
using Globe.Client.Platofrm.Events;
using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Shared.Services;
using MyLabLocalizer.Core.Services;
using MyLabLocalizer.Core.Services.Notifications;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MyLabLocalizer.Dialogs.ViewModels
{
    class SaveJoblistViewModel : BindableBase, IDialogAware
    {
        private readonly ILogService _logService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUserService _userService;
        private readonly IJobListManagementService _jobListManagementService;
        private readonly INotificationService _notificationService;

        private IEnumerable<BindableNotTranslatedConceptView> _notTranslatedConceptViews;
        private Language _language;

        public SaveJoblistViewModel(
            ILogService logService,
            IEventAggregator eventAggregator,
            IUserService userService,
            IJobListManagementService jobListManagementService,
            INotificationService notificationService)
        {
            _logService = logService;
            _eventAggregator = eventAggregator;
            _userService = userService;
            _jobListManagementService = jobListManagementService;
            _notificationService = notificationService;
        }

        public string Title
        {
            get { return DialogNames.SAVE_JOBLIST; }
        }

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            private set { SetProperty(ref _busy, value); }
        }

        private string _jobListName;
        public string JobListName
        {
            get { return _jobListName; }
            set { SetProperty(ref _jobListName, value); }
        }

        private IEnumerable<Models.BindableApplicationUser> _users;
        public IEnumerable<Models.BindableApplicationUser> Users
        {
            get { return _users; }
            private set { SetProperty(ref _users, value); }
        }

        private Models.BindableApplicationUser _selectedUser;
        public Models.BindableApplicationUser SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ??= new DelegateCommand(async () =>
            {
                try
                {
                    _eventAggregator.GetEvent<BusyChangedEvent>().Publish(true);
                    await _jobListManagementService.SaveAsync(JobListName, _notTranslatedConceptViews, SelectedUser, _language);
                    await _notificationService.NotifyAsync(new Notification
                    {
                        Title = "Joblist Status",
                        Message = $"New Joblist ({JobListName}) saved",
                        Level = NotificationLevel.Info
                    });
                    RaiseRequestClose(new DialogResult(ButtonResult.OK));                  
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    await _notificationService.NotifyAsync(new Notification
                    {
                        Title = "Joblist Status",
                        Message = "Joblist not saved",
                        Level = NotificationLevel.Error
                    });         
                }
                finally
                {
                    _eventAggregator.GetEvent<BusyChangedEvent>().Publish(false);
                }
            }, () => !string.IsNullOrWhiteSpace(JobListName));

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
                _language = parameters.GetValue<Language>(DialogParams.LANGUAGE);
                Users = (await _userService.GetUsersAsync(_language))
                    .Select(item => new BindableApplicationUser
                    {
                        Id = item.Id,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        UserName = item.UserName,
                        Email = item.Email
                    });
                _notTranslatedConceptViews = parameters.GetValue<IEnumerable<BindableNotTranslatedConceptView>>(DialogParams.NOT_TRANSLATED_CONCEPT_VIEWS);
            }
            catch (Exception e)
            {
                _logService.Exception(e);
                Users = null;
                _notTranslatedConceptViews = null;

                await _notificationService.NotifyAsync(new Notification
                {
                    Title = "Error",
                    Message = "Error during dialog opening",
                    Level = NotificationLevel.Error
                });

            }
            finally
            {
                Busy = false;
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(JobListName))
            {
                SaveCommand.RaiseCanExecuteChanged();           
            }
        }
    }
}
