using MyLabLocalizer.LocalizationService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IAsyncReadService<T, TKey>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(TKey key);
    }

    public interface IAsyncReadService<T> : IAsyncReadService<T, int>
    {
    }
}
