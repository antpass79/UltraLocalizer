using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.DTOs;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IAsyncConceptDetailsService
    {
        Task<ConceptDetailsDTO> GetAsync(JobListConcept jobListConcept);
    }
}
