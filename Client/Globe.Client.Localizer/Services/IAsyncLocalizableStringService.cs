using Globe.Client.Localizer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    public interface IAsyncLocalizableStringService
    {
        Task<IEnumerable<LocalizableString>> GetAllAsync();
        Task SaveAsync(IEnumerable<LocalizableString> strings);
    }
}
