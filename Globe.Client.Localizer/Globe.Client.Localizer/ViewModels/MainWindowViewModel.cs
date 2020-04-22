using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.ViewModels;
using Globe.Client.Platofrm.Events;
using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Globe.Client.Localizer.ViewModels
{
    internal class MainWindowViewModel : AuthorizeWindowViewModel
    {
        List<MenuOption> _allMenuOptions;

        private readonly IViewNavigationService _viewNavigationService;
        private readonly IAsyncLoginService _loginService;

        public MainWindowViewModel(IViewNavigationService viewNavigationService, IEventAggregator eventAggregator, IAsyncLoginService loginService)
            : base(eventAggregator)
        {
            _viewNavigationService = viewNavigationService;
            _loginService = loginService;

            eventAggregator.GetEvent<BusyChangedEvent>().Subscribe(busy =>
            {
                this.Busy = busy;
            });

            eventAggregator.GetEvent<StatusBarMessageChangedEvent>().Subscribe(statusBarMessage =>
            {
                this.StatusBarMessage = statusBarMessage;
            });

            this.StatusBarMessage = new StatusBarMessage
            {
                MessageType = MessageType.Information,
                Text = string.Empty
            };

            _allMenuOptions = new List<MenuOption>
            {
                new MenuOption
                {
                    Title = "Home",
                    IsSelected = true,
                    Roles = string.Empty,
                    AlwaysVisible = true,
                    ViewName = ViewNames.HOME_VIEW
                },
                new MenuOption
                {
                    Title = "Jobs",
                    IsSelected = false,
                    Roles = "Admin, UserManager",
                    ViewName = ViewNames.JOBS_VIEW
                },
                new MenuOption
                {
                    Title = "Merge",
                    IsSelected = false,
                    Roles = "Admin",
                    ViewName = ViewNames.MERGE_VIEW
                }
            };

            MenuOptions = _allMenuOptions;
        }

        IEnumerable<MenuOption> _menuOptions;
        public IEnumerable<MenuOption> MenuOptions
        {
            get => _menuOptions;
            set
            {
                SetProperty<IEnumerable<MenuOption>>(ref _menuOptions, value);
            }
        }

        bool _busy;
        public bool Busy
        {
            get => _busy;
            set
            {
                SetProperty<bool>(ref _busy, value);
            }
        }

        string _headerTitle;
        public string HeaderTitle
        {
            get => _headerTitle;
            set
            {
                SetProperty<string>(ref _headerTitle, value);
            }
        }

        StatusBarMessage _statusBarMessage;
        public StatusBarMessage StatusBarMessage
        {
            get => _statusBarMessage;
            set
            {
                SetProperty<StatusBarMessage>(ref _statusBarMessage, value);
            }
        }

        MenuOption _selectedMenuOption;
        public MenuOption SelectedMenuOption
        {
            get => _selectedMenuOption;
            set
            {
                SetProperty<MenuOption>(ref _selectedMenuOption, value);
            }
        }

        private DelegateCommand<MenuOption> _menuOptionCommand = null;
        public DelegateCommand<MenuOption> MenuOptionCommand =>
            _menuOptionCommand ?? (_menuOptionCommand = new DelegateCommand<MenuOption>((menuOption) =>
            {
                this.SelectedMenuOption = menuOption;
                _viewNavigationService.NavigateTo(menuOption.ViewName);
            }));

        private DelegateCommand _loginCommand = null;
        public DelegateCommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new DelegateCommand(() =>
            {
                _viewNavigationService.NavigateTo(ViewNames.LOGIN_VIEW);
            }));

        private DelegateCommand _logoutCommand = null;
        public DelegateCommand LogoutCommand =>
            _logoutCommand ?? (_logoutCommand = new DelegateCommand(async () =>
            {
                await _loginService.LogoutAsync(new Models.Credentials());
                _viewNavigationService.NavigateTo(ViewNames.HOME_VIEW);
            }));

        protected override void OnAuthenticationChanged(IPrincipal principal)
        {
            base.OnAuthenticationChanged(principal);

            List<MenuOption> menuOptions = new List<MenuOption>();

            foreach (var menuOption in _allMenuOptions)
            {
                if (menuOption.AlwaysVisible)
                {
                    menuOptions.Add(menuOption);
                    continue;
                }

                string[] roles = menuOption.Roles.Split(',', System.StringSplitOptions.RemoveEmptyEntries);
                foreach (var role in roles)
                {
                    if (principal.IsInRole(role))
                    {
                        menuOptions.Add(menuOption);
                        break;
                    }
                }
            }

            this.MenuOptions = menuOptions;

            this.HeaderTitle = BuildHeaderTitle();
        }

        private string BuildHeaderTitle()
        {
            if (!Identity.IsAuthenticated)
                return string.Empty;

            StringBuilder builder = new StringBuilder($"Hello {Identity.Name} ");

            if (this.UserRoles != null && this.UserRoles.Count() > 0)
            {
                builder.Append("[");

                foreach (var userRole in UserRoles)
                {
                    builder.Append($" {userRole} ");
                }

                builder.Append("]");
            }

            builder.Append("!");

            return builder.ToString();
        }
    }
}
