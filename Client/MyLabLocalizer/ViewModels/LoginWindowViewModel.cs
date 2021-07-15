using MyLabLocalizer.Models;
using MyLabLocalizer.Services;
using Globe.Client.Platofrm.Events;
using MyLabLocalizer.Core;
using MyLabLocalizer.Core.Extensions;
using MyLabLocalizer.Core.Services;
using MyLabLocalizer.Core.ViewModels;
using Prism.Commands;
using Prism.Events;
using System;
using System.Security;

namespace MyLabLocalizer.ViewModels
{
    internal class LoginWindowViewModel : LocalizeWindowViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IViewNavigationService _viewNavigationService;
        private readonly IAsyncLoginService _loginService;

        public LoginWindowViewModel(
            IIdentityStore identityStore,
            IEventAggregator eventAggregator,
            IViewNavigationService viewNavigationService,
            IAsyncLoginService loginService,
            ILocalizationAppService localizationAppService)
            : base(identityStore, eventAggregator, localizationAppService)
        {
            _eventAggregator = eventAggregator;
            _viewNavigationService = viewNavigationService;
            _loginService = loginService;
        }

        string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                SetProperty(ref _userName, value);
            }
        }

        SecureString _password;
        public SecureString Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
            }
        }

        LoginResult _loginResult = new LoginResult();
        public LoginResult LoginResult
        {
            get => _loginResult;
            set
            {
                SetProperty(ref _loginResult, value);
            }
        }

        private DelegateCommand _loginCommand = null;
        public DelegateCommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new DelegateCommand(async () =>
            {
                try
                {
                    _eventAggregator.GetEvent<BusyChangedEvent>().Publish(true);

                    LoginResult = new LoginResult();
                    LoginResult = await _loginService.LoginAsync(new Credentials
                    {
                        UserName = UserName,
                        Password = Password.ToPlainString()
                    });

                    ClearFields();
                    if (LoginResult.Successful)
                    {
                        _viewNavigationService.NavigateTo(ViewNames.JOBLIST_STATUS_VIEW, ViewNames.LOGIN_VIEW);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    _eventAggregator.GetEvent<BusyChangedEvent>().Publish(false);
                }
            }));

        private DelegateCommand _cancelCommand = null;
        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(() =>
            {
                ClearFields();
                LoginResult = new LoginResult();
                _viewNavigationService.NavigateTo(ViewNames.HOME_VIEW);
            }));

        private void ClearFields()
        {
            UserName = string.Empty;
            Password = new SecureString();
        }
    }
}
