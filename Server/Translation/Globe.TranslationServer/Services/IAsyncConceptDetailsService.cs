using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncConceptDetailsService
    {
        Task<ConceptDetailsDTO> GetAsync(JobListConcept jobListConcept);
    }
}
