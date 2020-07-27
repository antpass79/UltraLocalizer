using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Services
{
    public interface IGlobeDataStorage
    {
        Task StoreAsync(GlobeLocalStorageData data);
        Task<GlobeLocalStorageData> GetAsync();
        Task RemoveAsync();
    }
}
