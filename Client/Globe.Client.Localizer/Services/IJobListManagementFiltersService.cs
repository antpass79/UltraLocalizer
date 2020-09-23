using Globe.Client.Localizer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface IJobListManagementFiltersService
    {
        Task<IEnumerable<JobItem>> GetJobItemsAsync(string userName, string ISOCoding);
        Task<IEnumerable<Language>> GetLanguagesAsync();
    }
}
