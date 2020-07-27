using Blazored.LocalStorage;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Services
{
    public class GlobeInMemoryStorage : IGlobeDataStorage
    {
        GlobeLocalStorageData _data;

        async public Task StoreAsync(GlobeLocalStorageData data)
        {
            _data = data;
            await Task.CompletedTask;
        }

        async public Task<GlobeLocalStorageData> GetAsync()
        {
            if (_data == null)
                _data = new GlobeLocalStorageData();

            return await Task.FromResult(_data);
        }

        async public Task RemoveAsync()
        {
            _data = new GlobeLocalStorageData();
            await Task.CompletedTask;
        }
    }
}
