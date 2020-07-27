using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public interface IGlobeDataStorage
    {
        Task StoreAsync(GlobeLocalStorageData data);
        Task<GlobeLocalStorageData> GetAsync();
        Task RemoveAsync();
    }
}
