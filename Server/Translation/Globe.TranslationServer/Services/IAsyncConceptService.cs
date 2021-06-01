using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncConceptService
    {
        Task<ConceptDTO> GetConceptAsync(int id);
        Task SaveAsync(SavableConceptModelDTO savableConceptModel);
        Task<NewConceptsResult> CheckNewConceptsAsync();
    }
}
