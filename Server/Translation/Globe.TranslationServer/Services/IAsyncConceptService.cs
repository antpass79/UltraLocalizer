using Globe.TranslationServer.DTOs;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncConceptService
    {
        Task SaveAsync(SavableConceptModelDTO savableConceptModel);
        Task<bool> CheckNewConceptsAsync();
    }
}
