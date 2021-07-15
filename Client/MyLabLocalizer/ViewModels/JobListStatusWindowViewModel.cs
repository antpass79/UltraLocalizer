using MyLabLocalizer.Models;
using MyLabLocalizer.Services;
using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Shared.Services;
using MyLabLocalizer.Shared.Utilities;
using MyLabLocalizer.Core;
using MyLabLocalizer.Core.Localization;
using MyLabLocalizer.Core.Services;
using MyLabLocalizer.Core.Services.Notifications;
using MyLabLocalizer.Core.ViewModels;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyLabLocalizer.ViewModels
{
    internal class JobListStatusWindowViewModel : LocalizeWindowViewModel
    {
        private readonly ILogService _logService;
        private readonly INotificationService _notificationService;
        private readonly IJobListStatusFiltersService _jobListStatusFiltersService;
        private readonly IJobListStatusViewService _jobListStatusViewService;
        private readonly IJobListStatusChangeService _jobListStatusChangeService;
        private readonly IVisibilityFiltersService _visibilityFiltersService;
        private readonly IViewNavigationService _viewNavigationService;

        public JobListStatusWindowViewModel(
            IIdentityStore identityStore,
            IEventAggregator eventAggregator,
            ILogService logService,
            INotificationService notificationService,
            IJobListStatusFiltersService jobListStatusFiltersService,
            IJobListStatusViewService jobListStatusViewService,
            IJobListStatusChangeService jobListStatusChangeService,
            ILocalizationAppService localizationAppService,
            IVisibilityFiltersService visibilityFiltersService,
            IViewNavigationService viewNavigationService)
            : base(identityStore, eventAggregator, localizationAppService)
        {
            _logService = logService;
            _notificationService = notificationService;
            _jobListStatusFiltersService = jobListStatusFiltersService;
            _jobListStatusViewService = jobListStatusViewService;
            _jobListStatusChangeService = jobListStatusChangeService;
            _visibilityFiltersService = visibilityFiltersService;
            _viewNavigationService = viewNavigationService;
        }

        bool _conceptDetailsBusy;
        public bool ConceptDetailsBusy
        {
            get => _conceptDetailsBusy;
            set
            {
                SetProperty(ref _conceptDetailsBusy, value);
            }
        }

        bool _gridBusy;
        public bool GridBusy
        {
            get => _gridBusy;
            set
            {
                SetProperty(ref _gridBusy, value);
            }
        }

        bool _filtersBusy;
        public bool FiltersBusy
        {
            get => _filtersBusy;
            set
            {
                SetProperty(ref _filtersBusy, value);
            }
        }

        int _itemCount;

        public int ItemCount
        {
            get => _itemCount;
            set
            {
                SetProperty(ref _itemCount, value);
            }
        }

        bool _showFilters = true;
        public bool ShowFilters
        {
            get => _showFilters;
            set
            {
                _visibilityFiltersService.Visible = value;
                SetProperty(ref _showFilters, value);
            }
        }

        private bool _isMasterTranslator = false;
        public bool IsMasterTranslator
        {
            get { return _isMasterTranslator; }
            set { SetProperty(ref _isMasterTranslator, value); }
        }

        IEnumerable<BindableJobListStatus> _jobListStatuses;
        public IEnumerable<BindableJobListStatus> JobListStatuses
        {
            get => _jobListStatuses;
            set
            {
                SetProperty(ref _jobListStatuses, value);
            }
        }

        BindableJobListStatus _selectedJobListStatus;
        public BindableJobListStatus SelectedJobListStatus
        {
            get => _selectedJobListStatus;
            set
            {
                SetProperty(ref _selectedJobListStatus, value);
            }
        }

        IEnumerable<BindableApplicationUser> _applicationUsers;
        public IEnumerable<BindableApplicationUser> ApplicationUsers
        {
            get => _applicationUsers;
            set
            {
                SetProperty(ref _applicationUsers, value);
            }
        }

        BindableApplicationUser _selectedApplicationUser;
        public BindableApplicationUser SelectedApplicationUser
        {
            get => _selectedApplicationUser;
            set
            {
                SetProperty(ref _selectedApplicationUser, value);
            }
        }

        IEnumerable<Language> _languages;
        public IEnumerable<Language> Languages
        {
            get => _languages;
            set
            {
                SetProperty(ref _languages, value);
            }
        }

        Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                SetProperty(ref _selectedLanguage, value);
            }
        }

        IEnumerable<JobList> _jobListViews;
        public IEnumerable<JobList> JobListViews
        {
            get => _jobListViews;
            set
            {
                SetProperty(ref _jobListViews, value);
            }
        }

        private DelegateCommand _searchCommand = null;
        public DelegateCommand SearchCommand =>
            _searchCommand ??= new DelegateCommand(async () =>
            {
                await OnSearch();
            });

        private DelegateCommand<JobList> _goToJobListCommand = null;
        public DelegateCommand<JobList> GoToJobListCommand =>
            _goToJobListCommand ??= new DelegateCommand<JobList>((jobList) =>
            {
                var jobListSearch = new JobListConceptSearch
                {
                    JobListId = jobList.Id,
                    LanguageId = jobList.LanguageId
                };
                _viewNavigationService.NavigateTo(ViewNames.CURRENT_JOB_VIEW, jobListSearch);
            });

        private DelegateCommand<JobList> _fromAssignedToClosedCommand = null;
        public DelegateCommand<JobList> FromAssignedToClosedCommand =>
            _fromAssignedToClosedCommand ??= new DelegateCommand<JobList>((jobList) =>
            {
                var result = MessageBox.Show("Are you Sure?\nJoblist status will change from Assigned To Closed", "UltraLocalizer", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    jobList.NextStatus.Description = "Closed";
                    _jobListStatusChangeService.SaveAsync(jobList);
                }

            });

        private DelegateCommand<JobList> _fromToBeRevisedToClosedCommand = null;
        public DelegateCommand<JobList> FromToBeRevisedToClosedCommand =>
            _fromToBeRevisedToClosedCommand ??= new DelegateCommand<JobList>((jobList) =>
            {
                var result = MessageBox.Show("Are you Sure?\nJoblist status will change from ToBeRevised To Closed", "UltraLocalizer", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    jobList.NextStatus.Description = "Closed";
                    _jobListStatusChangeService.SaveAsync(jobList);
                }
            });

        private DelegateCommand<JobList> _fromClosedToSavedCommand = null;
        public DelegateCommand<JobList> FromClosedToSavedCommand =>
            _fromClosedToSavedCommand ??= new DelegateCommand<JobList>((jobList) =>
            {
                var result = MessageBox.Show("Are you Sure?\nJoblist status will change from Closed To Saved", "UltraLocalizer", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    jobList.NextStatus.Description = "Saved";
                    _jobListStatusChangeService.SaveAsync(jobList);
                }
            });

        private DelegateCommand<JobList> _fromClosedToToBeRevisedCommand = null;
        public DelegateCommand<JobList> FromClosedToToBeRevisedCommand =>
            _fromClosedToToBeRevisedCommand ??= new DelegateCommand<JobList>((jobList) =>
            {
                var result = MessageBox.Show("Are you Sure?\nJoblist status will change from Closed To ToBeRevised", "UltraLocalizer", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    jobList.NextStatus.Description = "ToBeRevised";
                    _jobListStatusChangeService.SaveAsync(jobList);
                }
            });

        private DelegateCommand<JobList> _fromSavedToDeletedCommand = null;
        public DelegateCommand<JobList> FromSavedToDeletedCommand =>
            _fromSavedToDeletedCommand ??= new DelegateCommand<JobList>((jobList) =>
            {
                var result = MessageBox.Show("Are you Sure?\nJoblist status will change from Saved To Deleted", "UltraLocalizer", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    jobList.NextStatus.Description = "Deleted";
                    _jobListStatusChangeService.SaveAsync(jobList);
                }
            });

        async protected override Task OnLoad(string fromView, object data)
        {
            await InitializeFilters(fromView, data);
        }

        protected override Task OnUnload()
        {
            ClearPage();
            return Task.CompletedTask;
        }

        async private Task InitializeFilters(string fromView, object data)
        {
            this.FiltersBusy = true;

            try
            {
                ShowFilters = _visibilityFiltersService.Visible;

                JobListStatuses = await _jobListStatusFiltersService.GetJobListStatusesAsync();
                ApplicationUsers = await _jobListStatusFiltersService.GetApplicationUsersAsync(Identity.Name);
                Languages = await _jobListStatusFiltersService.GetLanguagesAsync();

                SelectedLanguage = this.Languages.FirstOrDefault();
                SelectedJobListStatus = this.JobListStatuses.FirstOrDefault();
                SelectedApplicationUser = this.ApplicationUsers.FirstOrDefault();
                //SelectedApplicationUser = this.ApplicationUsers.Where(item => item.UserName == Identity.Name).FirstOrDefault();
                IsMasterTranslator = UserRoles.Contains(Roles.MasterTranslator);

                if(fromView == ViewNames.LOGIN_VIEW)
                {
                    ShowFilters = false;
                    this.SearchCommand.Execute();
                }
            }
            catch (Exception e)
            {
                _logService.Exception(e);
                await _notificationService.NotifyAsync(new Notification
                {
                    Title = Localize[LanguageKeys.Error],
                    Message = Localize[LanguageKeys.Error_during_filters_initialization],
                    Level = NotificationLevel.Error
                });
            }
            finally
            {
                this.FiltersBusy = false;
            }
        }

        private async Task OnSearch()
        {
            GridBusy = true;
            ItemCount = 0;
            JobListViews = null;

            try
            {
                if (
                    SelectedJobListStatus == null ||
                    SelectedApplicationUser == null ||
                    SelectedLanguage == null)
                {
                    JobListViews = null;
                }
                else
                {
                    JobListViews = await _jobListStatusViewService.GetJobListViewsAsync(
                        new JobListSearch
                        {
                            LanguageId = SelectedLanguage.Id,
                            JobListStatus = SelectedJobListStatus.Description,
                            //JobListStatusId = string.IsNullOrWhiteSpace(SelectedJobListStatus) ? default(JobListStatus) : Enum.Parse<JobListStatus>(SelectedJobListStatus),
                            UserName = SelectedApplicationUser.UserName
                        });

                    ItemCount = JobListViews.Count();
                }
            }
            catch (Exception exception)
            {
                _logService.Exception(exception);
                await _notificationService.NotifyAsync(Localize[LanguageKeys.Error], Localize[LanguageKeys.Error_during_concepts_request], NotificationLevel.Error);
            }
            finally
            {
                this.GridBusy = false;
            }
        }

        private void ClearPage()
        {
            JobListViews = null;
            ItemCount = 0;
        }
    }
}
