using MyLabLocalizer.Shared.DTOs;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IAsyncNotificationService
    {
        Task JoblistChanged(string joblistName);

        Task ConceptsChanged(NewConceptsResult result);
    }
}
