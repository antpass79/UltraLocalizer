using Globe.Shared.DTOs;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncNotificationService
    {
        Task JoblistChanged(string joblistName);

        Task ConceptsChanged(NewConceptsResult result);
    }
}
