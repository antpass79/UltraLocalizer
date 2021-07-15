using MyLabLocalizer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    public interface IAsyncLocalizableStringService
    {
        Task<IEnumerable<LocalizableString>> GetAllAsync();
        Task SaveAsync(IEnumerable<LocalizableString> strings);
    }
}
