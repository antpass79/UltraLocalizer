using MyLabLocalizer.Models;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    interface IJobListStatusChangeService
    {
        Task SaveAsync(JobList jobList);
    }
}
