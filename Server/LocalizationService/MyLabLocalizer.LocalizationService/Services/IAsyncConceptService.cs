using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.DTOs;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IAsyncConceptService
    {
        Task<ConceptDTO> GetConceptAsync(int id);
        Task SaveAsync(SavableConceptModelDTO savableConceptModel);
        Task UpdateAsync(TranslatedString translatedString);
        Task<NewConceptsResult> CheckNewConceptsAsync();
    }
}
