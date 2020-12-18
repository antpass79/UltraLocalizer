using Globe.Client.Localizer.Models;
using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    interface ICurrentJobConceptViewService
    {
        Task<IEnumerable<JobListConcept>> GetConceptViewsAsync(JobListConceptSearch search);
        Task<ConceptDetails> GetConceptDetailsAsync(JobListConcept jobListConcept);
    }
}
