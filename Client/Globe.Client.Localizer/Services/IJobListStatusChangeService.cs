using Globe.Client.Localizer.Models;
using Globe.Shared.DTOs;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IJobListStatusChangeService
    {
        Task SaveAsync(JobList jobList);
    }
}
