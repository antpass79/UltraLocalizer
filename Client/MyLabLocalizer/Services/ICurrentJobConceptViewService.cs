using MyLabLocalizer.Models;
using MyLabLocalizer.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    interface ICurrentJobConceptViewService
    {
        Task<IEnumerable<JobListConcept>> GetConceptViewsAsync(JobListConceptSearch search);
        Task<ConceptDetails> GetConceptDetailsAsync(JobListConcept jobListConcept);
    }
}
