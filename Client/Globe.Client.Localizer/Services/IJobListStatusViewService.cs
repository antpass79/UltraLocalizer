using Globe.Client.Localizer.Models;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IJobListStatusViewService
    {
        Task<IEnumerable<JobList>> GetJobListViewsAsync(JobListSearch search);
    }
}
