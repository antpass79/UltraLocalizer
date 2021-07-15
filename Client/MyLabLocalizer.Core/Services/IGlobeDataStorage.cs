using System.Threading.Tasks;

namespace MyLabLocalizer.Core.Services
{
    public interface IGlobeDataStorage
    {
        Task StoreAsync(GlobeLocalStorageData data);
        Task<GlobeLocalStorageData> GetAsync();
        Task RemoveAsync();
    }
}
