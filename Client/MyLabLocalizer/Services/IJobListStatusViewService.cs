using MyLabLocalizer.Models;
using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    interface IJobListStatusViewService
    {
        Task<IEnumerable<JobList>> GetJobListViewsAsync(JobListSearch search);
    }
}
