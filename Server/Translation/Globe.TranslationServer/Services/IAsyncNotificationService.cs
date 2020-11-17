using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncNotificationService
    {
        Task JoblistChanged(string joblistName);

        Task ConceptsChanged(int count);
    }
}
