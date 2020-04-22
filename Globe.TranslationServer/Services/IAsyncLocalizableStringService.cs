using Globe.TranslationServer.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncLocalizableStringService
    {
        Task<IEnumerable<LocalizableString>> GetAllAsync();
        Task SaveAsync(IEnumerable<LocalizableString> strings);
    }
}
