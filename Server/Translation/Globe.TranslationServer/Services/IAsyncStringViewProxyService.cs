using Globe.TranslationServer.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncStringViewProxyService
    {
        Task<IEnumerable<StringViewDTO>> GetAllAsync(StringViewSearchDTO search);
    }
}
