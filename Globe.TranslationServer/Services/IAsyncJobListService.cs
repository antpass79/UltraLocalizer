using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncJobListService : IAsyncService<JobList>
    {
        Task<IEnumerable<JobList>> GetAllAsync(string userName, string ISOCoding);
    }
}
