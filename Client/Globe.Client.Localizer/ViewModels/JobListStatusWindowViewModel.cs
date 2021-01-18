using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Assets.Localization;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.ViewModels;
using Globe.Shared.DTOs;
using Globe.Shared.Utilities;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.ViewModels
{
    internal class JobListStatusWindowViewModel : LocalizeWindowViewModel
    {
        private readonly ILoggerService _loggerService;
        private readonly INotificationService _notificationService;
        private readonly IJobListStatusFiltersService _jobListStatusFiltersService;
        private readonly IJobListStatusViewService _jobListStatusViewService;

        public JobListStatusWindowViewModel(
            IEventAggregator eventAggregator,
            ILoggerService loggerService,
            INotificationService notificationService,
            IJobListStatusFiltersService jobListStatusFiltersService,
            IJobListStatusViewService jobListStatusViewService,
            ILocalizationAppService localizationAppService)
            : base(eventAggregator, localizationAppService)
        {
            _loggerService = loggerService;
            _notificationService = notificationService;
            _jobListStatusFiltersService = jobListStatusFiltersService;
            _jobListStatusViewService = jobListStatusViewService;
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

        string _stringInserted = string.Empty;
        public string StringInserted
        {
            get => _stringInserted;
            set 
            { 
                SetProperty(ref _stringInserted, value); 
            }
        }

        string _conceptInserted = string.Empty;
        public string ConceptInserted
        {
            get => _conceptInserted;
            set
            {
                SetProperty(ref _conceptInserted, value);
            }
        }

        IEnumerable<JobItem> _jobLists;
        public IEnumerable<JobItem> JobLists
        {
            get => _jobLists;
            set
            {
                SetProperty(ref _jobLists, value);
            }
        }

        JobItem _selectedJobList;
        public JobItem SelectedJobList
        {
            get => _selectedJobList;
            set
            {
                SetProperty(ref _selectedJobList, value);
            }
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
            _searchCommand ?? (_searchCommand = new DelegateCommand(async () =>
            {
                await OnSearch();
            }));

        private DelegateCommand _userNameChangeCommand = null;
        public DelegateCommand UserNameChangeCommand =>
            _userNameChangeCommand ?? (_userNameChangeCommand = new DelegateCommand(async () =>
            {
                this.FiltersBusy = true;

                this.JobLists = await _jobListStatusFiltersService.GetJobListsAsync(this.SelectedApplicationUser != null ? this.SelectedApplicationUser.UserName : SharedConstants.USER_NAME_ALL);
                this.SelectedJobList = this.JobLists.FirstOrDefault();

                this.FiltersBusy = false;
            }));

        async protected override Task OnLoad()
        {
            await InitializeFilters();
        }

        protected override Task OnUnload()
        {
            ClearPage();
            return Task.CompletedTask;
        }

        async private Task InitializeFilters()
        {
            this.FiltersBusy = true;

            try
            {
                JobListStatuses = await _jobListStatusFiltersService.GetJobListStatusesAsync();
                ApplicationUsers = await _jobListStatusFiltersService.GetApplicationUsersAsync();
                JobLists = await _jobListStatusFiltersService.GetJobListsAsync(SelectedApplicationUser != null ? SelectedApplicationUser.UserName : SharedConstants.USER_NAME_ALL);
                Languages = await _jobListStatusFiltersService.GetLanguagesAsync();

                SelectedJobListStatus = this.JobListStatuses.FirstOrDefault();
                SelectedApplicationUser = this.ApplicationUsers.FirstOrDefault();
                SelectedJobList = this.JobLists.FirstOrDefault();
                SelectedLanguage = this.Languages.FirstOrDefault();
            }
            catch (Exception e)
            {
                _loggerService.Exception(e);
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
                    SelectedJobList == null ||
                    SelectedLanguage == null)
                {
                    JobListViews = null;
                }
                else
                {
                    JobListViews = await _jobListStatusViewService.GetJobListViewsAsync(
                        new JobListSearch
                        {   
                            //Forse il BindableJobListStatus non ha senso di esistere?
                            LanguageId = SelectedLanguage.Id,
                            JobListName = SelectedJobList.Name,
                            //JobListStatusId = string.IsNullOrWhiteSpace(SelectedJobListStatus) ? default(JobListStatus) : Enum.Parse<JobListStatus>(SelectedJobListStatus),
                            UserName = SelectedApplicationUser.UserName
                        });

                    ItemCount = JobListViews.Count();
                }
            }
            catch (Exception exception)
            {
                _loggerService.Exception(exception);
                await _notificationService.NotifyAsync(Localize[LanguageKeys.Error], Localize[LanguageKeys.Error_during_concepts_request], Platform.Services.Notifications.NotificationLevel.Error);
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
