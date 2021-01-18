using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform;
using Globe.Client.Platform.Assets.Localization;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.Services.Notifications;
using Globe.Client.Platform.ViewModels;
using Globe.Client.Platofrm.Events;
using Globe.Shared.Utilities;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.ViewModels
{
    internal class MainWindowViewModel : LocalizeWindowViewModel
    {
        List<MenuOption> _allMenuOptions = new List<MenuOption>();
        List<LanguageOption> _allLanguageOptions = new List<LanguageOption>();
        HubConnection connection;

        private readonly IViewNavigationService _viewNavigationService;
        private readonly IAsyncLoginService _loginService;
        private readonly IGlobeDataStorage _globeDataStorage;
        private readonly ISettingsService _settingsService;

        public MainWindowViewModel(
            IViewNavigationService viewNavigationService,
            IEventAggregator eventAggregator,
            IAsyncLoginService loginService,
            IGlobeDataStorage globeDataStorage,
            ILocalizationAppService localizationAppService,
            INotificationService notificationService,
            ISettingsService settingsService)
            : base(eventAggregator, localizationAppService)
        {
            _viewNavigationService = viewNavigationService;
            _loginService = loginService;
            _globeDataStorage = globeDataStorage;
            NotificationService = notificationService;
            _settingsService = settingsService;

            eventAggregator.GetEvent<BusyChangedEvent>().Subscribe(busy =>
            {
                this.Busy = busy;
            });
            eventAggregator.GetEvent<BackgroundBusyChangedEvent>().Subscribe(backgroundBusy =>
            {
                this.BackgroundBusy = backgroundBusy;
            });

            eventAggregator.GetEvent<ViewNavigationChangedEvent>().Subscribe(viewNavigation =>
            {
                MenuOptions
                .ToList()
                .ForEach(menuOption => menuOption.IsSelected = viewNavigation.ToView == menuOption.ViewName);
                SelectedMenuOption = MenuOptions.SingleOrDefault(menuOption => menuOption.IsSelected);
            });

            _allLanguageOptions = new List<LanguageOption>
            {
                new LanguageOption
                {
                    Title = LanguageKeys.English,
                    IsSelected = true,
                    Language = SharedConstants.LANGUAGE_EN
                },
                new LanguageOption
                {
                    Title = LanguageKeys.Italian,
                    IsSelected = false,
                    Language = SharedConstants.LANGUAGE_IT
                },
            };

            LanguageOptions = _allLanguageOptions;

            ChangeLanguage(_allLanguageOptions[0].Language);
            SelectedLanguageOption = _allLanguageOptions[0];

            _allMenuOptions = new List<MenuOption>
            {
                new MenuOption
                {
                    Title = Localize[LanguageKeys.Page_Home],
                    TitleKey = LanguageKeys.Page_Home,
                    IconName = "home",
                    IsSelected = true,
                    Roles = Rules.All,
                    AlwaysVisible = true,
                    ViewName = ViewNames.HOME_VIEW
                },
                new MenuOption
                {
                    Title = Localize[LanguageKeys.Page_Joblist_Management],
                    TitleKey = LanguageKeys.Page_Joblist_Management,
                    IconName = "management",
                    IsSelected = false,
                    Roles = Rules.Group_Administrators,
                    AlwaysVisible = false,
                    ViewName = ViewNames.JOBLIST_MANAGEMENT_VIEW
                },
                new MenuOption
                {
                    Title = Localize[LanguageKeys.Page_Current_Job],
                    TitleKey = LanguageKeys.Page_Current_Job,
                    IconName = "current_job",
                    IsSelected = false,
                    Roles = Rules.Group_All,
                    AlwaysVisible = false,
                    ViewName = ViewNames.CURRENT_JOB_VIEW
                },
                new MenuOption
                {
                    Title = Localize[LanguageKeys.Page_Concept_Management],
                    TitleKey = LanguageKeys.Page_Concept_Management,
                    IconName = "translation",
                    IsSelected = false,
                    Roles = Rules.Group_All,
                    AlwaysVisible = false,
                    ViewName = ViewNames.CONCEPT_MANAGEMENT_VIEW
                },
                new MenuOption
                {
                    Title = Localize[LanguageKeys.Page_JobList_Status],
                    TitleKey = LanguageKeys.Page_JobList_Status,
                    IconName = "joblist_status",
                    IsSelected = false,
                    Roles = Rules.Group_All,
                    AlwaysVisible = false,
                    ViewName = ViewNames.JOBLIST_STATUS_VIEW
                }
                //new MenuOption
                //{
                //    Title = Localize[LanguageKeys.Translation],
                //    TitleKey = LanguageKeys.Translation,
                //    IconName = "work_in_progress",
                //    IsSelected = false,
                //    Roles = "Admin, UserManager",
                //    ViewName = ViewNames.JOBS_VIEW
                //},
                //new MenuOption
                //{
                //    Title = Localize[LanguageKeys.Merge],
                //    TitleKey = LanguageKeys.Merge,
                //    IconName = "work_in_progress",
                //    IsSelected = false,
                //    Roles = "Admin",
                //    ViewName = ViewNames.MERGE_VIEW
                //}
            };

            MenuOptions = _allMenuOptions;
            SelectedMenuOption = _allMenuOptions[0];
        }

        public INotificationService NotificationService { get; }

        IEnumerable<MenuOption> _menuOptions;
        public IEnumerable<MenuOption> MenuOptions
        {
            get => _menuOptions;
            set
            {
                SetProperty(ref _menuOptions, value);
            }
        }

        IEnumerable<LanguageOption> _languageOptions;
        public IEnumerable<LanguageOption> LanguageOptions
        {
            get => _languageOptions;
            set
            {
                SetProperty(ref _languageOptions, value);
            }
        }

        bool _isMenuOpen;
        public bool IsMenuOpen
        {
            get => _isMenuOpen;
            set
            {
                SetProperty(ref _isMenuOpen, value);
            }
        }

        bool _busy;
        public bool Busy
        {
            get => _busy;
            set
            {
                SetProperty(ref _busy, value);
            }
        }
        bool _backgroundBusy;
        public bool BackgroundBusy
        {
            get => _backgroundBusy;
            set
            {
                SetProperty(ref _backgroundBusy, value);
            }
        }

        string _headerTitle;
        public string HeaderTitle
        {
            get => _headerTitle;
            set
            {
                SetProperty(ref _headerTitle, value);
            }
        }

        MenuOption _selectedMenuOption;
        public MenuOption SelectedMenuOption
        {
            get => _selectedMenuOption;
            set
            {
                SetProperty(ref _selectedMenuOption, value);
            }
        }

        LanguageOption _selectedLanguageOption;
        public LanguageOption SelectedLanguageOption
        {
            get => _selectedLanguageOption;
            set
            {
                SetProperty(ref _selectedLanguageOption, value);
            }
        }

        private DelegateCommand<MenuOption> _menuOptionCommand = null;
        public DelegateCommand<MenuOption> MenuOptionCommand =>
            _menuOptionCommand ?? (_menuOptionCommand = new DelegateCommand<MenuOption>((menuOption) =>
            {
                this.SelectedMenuOption = menuOption;
                _viewNavigationService.NavigateTo(menuOption.ViewName);
            }));

        private DelegateCommand<LanguageOption> _languageOptionCommand = null;
        public DelegateCommand<LanguageOption> LanguageOptionCommand =>
            _languageOptionCommand ?? (_languageOptionCommand = new DelegateCommand<LanguageOption>((languageOption) =>
            {
                this.SelectedLanguageOption = languageOption;
                ChangeLanguage(languageOption.Language);
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

        private DelegateCommand<string> _navigateToCommand = null;
        public DelegateCommand<string> NavigateToCommand =>
            _navigateToCommand ?? (_navigateToCommand = new DelegateCommand<string>((navigateTo) =>
            {
                _viewNavigationService.NavigateTo(navigateTo);
            }));

        private DelegateCommand _removeAllNotificationsCommand = null;
        public DelegateCommand RemoveAllNotificationsCommand =>
            _removeAllNotificationsCommand ?? (_removeAllNotificationsCommand = new DelegateCommand(() =>
            {
                NotificationService.Clear();
            }));

        private DelegateCommand<Notification> _removeNotificationCommand = null;
        public DelegateCommand<Notification> RemoveNotificationCommand =>
            _removeNotificationCommand ?? (_removeNotificationCommand = new DelegateCommand<Notification>((notification) =>
            {
                NotificationService.Notifications.Remove(notification);
            }));

        protected override void OnAuthenticationChanged(IPrincipal principal)
        {
            base.OnAuthenticationChanged(principal);
            
            UpdateSignalR();

            List<MenuOption> menuOptions = new List<MenuOption>();

            foreach (var menuOption in _allMenuOptions)
            {
                if (menuOption.AlwaysVisible)
                {
                    menuOptions.Add(menuOption);
                    continue;
                }

                string[] roles = menuOption.Roles
                    .Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                    .Select(item => item.Trim())
                    .ToArray();
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

        async protected override Task OnLanguageChanged(string language)
        {
            await base.OnLanguageChanged(language);

            this.MenuOptions = BuildMenu(this.Principal);
            this.HeaderTitle = BuildHeaderTitle();
        }

        private IEnumerable<MenuOption> BuildMenu(IPrincipal principal)
        {
            List<MenuOption> menuOptions = new List<MenuOption>();

            foreach (var menuOption in _allMenuOptions)
            {
                menuOption.Title = Localize[menuOption.TitleKey];
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

            return menuOptions;
        }

        private string BuildHeaderTitle()
        {
            if (!Identity.IsAuthenticated)
                return string.Empty;

            return $"{Identity.Name} {Localize[LanguageKeys.Is_logged]}";
        }

        private async void UpdateSignalR()
        {
            if(!this.IsAuthenticated)
            {                  
                if (connection != null && connection.State == HubConnectionState.Connected)
                { 
                    await connection.StopAsync();
                    connection = null;
                }
                NotificationService.Clear();

                return;
            }

            connection = new HubConnectionBuilder()
                    .WithUrl(_settingsService.GetNotificationHubAddress(), options =>
                    {
                        options.AccessTokenProvider = async () => (await _globeDataStorage.GetAsync()).Token;

                        var handler = new HttpClientHandler
                        {
                            ClientCertificateOptions = ClientCertificateOption.Manual,
                            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
                        };
                        options.HttpMessageHandlerFactory = _ => handler;
                        options.WebSocketConfiguration = sockets =>
                        {
                            sockets.RemoteCertificateValidationCallback = (sender, certificate, chain, policyErrors) => true;
                        };
                    })
                    .Build();

            connection.On<string>(EventNames.JoblistChanged, async (jobListName) =>
            {
                var notification = new JobListStatusNotification { Message = jobListName };
                await NotificationService.NotifyAsync(notification);
            });

            connection.On<int>(EventNames.ConceptsChanged, async (conceptCount) =>
            {
                var notification = new ConceptsChangedNotification { Message = conceptCount.ToString() };
                await NotificationService.NotifyAsync(notification);
            });

            connection.On<string>(EventNames.SendAsync, async (message) =>
            {
                await NotificationService.NotifyAsync(new Notification
                {
                    Title = "Server Says:",
                    Message = message,
                    Level = NotificationLevel.Error
                });
            });

            await connection.StartAsync();        
        }
    }
}
