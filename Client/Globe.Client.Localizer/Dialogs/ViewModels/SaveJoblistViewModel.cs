using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Controls;
using Globe.Client.Platform.Services;
using Prism.Commands;
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
        private readonly IUserService _userService;
        private readonly IJobListManagementService _jobListManagementService;

        private IEnumerable<NotTranslatedConceptView> _notTranslatedConceptViews;
        private Language _language;

        public SaveJoblistViewModel(
            ILoggerService loggerService,
            IUserService userService,
            IJobListManagementService jobListManagementService)
        {
            _loggerService = loggerService;
            _userService = userService;
            _jobListManagementService = jobListManagementService;
        }

        private string _title = DialogNames.SAVE_JOBLIST;
        public string Title
        {
            get { return _title; }
            private set { SetProperty(ref _title, value); }
        }

        private string _jobListName;
        public string JobListName
        {
            get { return _jobListName; }
            set { SetProperty(ref _jobListName, value); }
        }

        private IEnumerable<ApplicationUser> _users;
        public IEnumerable<ApplicationUser> Users
        {
            get { return _users; }
            private set { SetProperty(ref _users, value); }
        }

        private ApplicationUser _selectedUser;
        public ApplicationUser SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(() =>
            {
                _jobListManagementService.SaveAsync(JobListName, _notTranslatedConceptViews, SelectedUser, _language);
            }, () => !string.IsNullOrWhiteSpace(JobListName)));

        private DelegateCommand<string> _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(parameter =>
            {
                ButtonResult result = ButtonResult.None;

                if (parameter?.ToLower() == "true")
                    result = ButtonResult.OK;
                else if (parameter?.ToLower() == "false")
                    result = ButtonResult.Cancel;

                RaiseRequestClose(new DialogResult(result));
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
            _language = parameters.GetValue<Language>("language");
            Users = await _userService.GetUsersAsync(_language);
            _notTranslatedConceptViews = parameters.GetValue<IEnumerable<NotTranslatedConceptView>>("notTranslatedConceptViews");
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
