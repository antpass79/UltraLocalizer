using Globe.TranslationServer.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncJobItemService : IAsyncService<JobItemDTO>
    {
        Task<IEnumerable<JobItemDTO>> GetAllAsync(string userName, string ISOCoding, bool isInAdministratorGroup);
    }
}
