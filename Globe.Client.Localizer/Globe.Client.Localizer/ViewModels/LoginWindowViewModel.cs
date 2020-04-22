using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform;
using Globe.Client.Platform.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Globe.Client.Localizer.ViewModels
{
    internal class LoginWindowViewModel : BindableBase
    {
        private readonly IViewNavigationService _viewNavigationService;
        private readonly IRegionManager _regionManager;
        private readonly IAsyncLoginService _loginService;

        public LoginWindowViewModel(IViewNavigationService viewNavigationService, IAsyncLoginService loginService)
        {
            _viewNavigationService = viewNavigationService;
            _loginService = loginService;
        }

        string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                SetProperty<string>(ref _userName, value);
            }
        }
        string _password;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty<string>(ref _password, value);
            }
        }
        LoginResult _loginResult = new LoginResult();
        public LoginResult LoginResult
        {
            get => _loginResult;
            set
            {
                SetProperty<LoginResult>(ref _loginResult, value);
            }
        }

        private DelegateCommand _loginCommand = null;
        public DelegateCommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new DelegateCommand(async () =>
            {
                LoginResult = new LoginResult();
                LoginResult = await _loginService.LoginAsync(new Credentials
                {
                    UserName = this.UserName,
                    Password = this.Password
                });

                ClearFields();
                if (LoginResult.Successful)
                {
                    _viewNavigationService.NavigateTo(ViewNames.HOME_VIEW);
                }
            }));

        private DelegateCommand _cancelCommand = null;
        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(() =>
            {
                ClearFields();
                _viewNavigationService.NavigateTo(ViewNames.HOME_VIEW);
            }));

        private void ClearFields()
        {
            this.UserName = string.Empty;
            this.Password = string.Empty;
        }
    }
}
