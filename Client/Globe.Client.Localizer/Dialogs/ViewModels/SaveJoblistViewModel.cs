using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.Services.Notifications;
using Globe.Client.Platofrm.Events;
using Globe.Shared.DTOs;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Globe.Client.Localizer.Dialogs.ViewModels
{
    class SaveJoblistViewModel : BindableBase, IDialogAware
    {
        private readonly ILoggerService _loggerService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUserService _userService;
        private readonly IJobListManagementService _jobListManagementService;
        private readonly INotificationService _notificationService;

        private IEnumerable<BindableNotTranslatedConceptView> _notTranslatedConceptViews;
        private Language _language;

        public SaveJoblistViewModel(
            ILoggerService loggerService,
            IEventAggregator eventAggregator,
            IUserService userService,
            IJobListManagementService jobListManagementService,
            INotificationService notificationService)
        {
            _loggerService = loggerService;
            _eventAggregator = eventAggregator;
            _userService = userService;
            _jobListManagementService = jobListManagementService;
            _notificationService = notificationService;
        }

        private string _title = DialogNames.SAVE_JOBLIST;
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
            _saveCommand ?? (_saveCommand = new DelegateCommand(async () =>
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
            }, () => !string.IsNullOrWhiteSpace(JobListName)));

        private DelegateCommand _closeDialogCommand;
        public DelegateCommand CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand(() =>
            {
                RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
            }));

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
                _loggerService.Exception(e);
                Users = null;
                _notTranslatedConceptViews = null;
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
