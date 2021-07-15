using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Client.Services
{
    public interface IGlobeDataStorage
    {
        Task StoreAsync(GlobeLocalStorageData data);
        Task<GlobeLocalStorageData> GetAsync();
        Task RemoveAsync();
    }
}
