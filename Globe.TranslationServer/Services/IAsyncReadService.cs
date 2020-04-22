using Globe.TranslationServer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
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
