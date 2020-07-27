using Blazored.LocalStorage;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Services
{
    public class GlobeLocalStorage : IGlobeDataStorage
    {
        static string GLOBE_STORAGE_DATA = "GLOBE_STORAGE_DATA";

        private readonly ILocalStorageService _localStorage;

        public GlobeLocalStorage(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        async public Task StoreAsync(GlobeLocalStorageData data)
        {
            await _localStorage.SetItemAsync(GLOBE_STORAGE_DATA, data);
        }

        async public Task<GlobeLocalStorageData> GetAsync()
        {
            var data = await _localStorage.GetItemAsync<GlobeLocalStorageData>(GLOBE_STORAGE_DATA);
            return data != null ? data : new GlobeLocalStorageData();
        }

        async public Task RemoveAsync()
        {
            await _localStorage.RemoveItemAsync(GLOBE_STORAGE_DATA);
        }
    }
}
