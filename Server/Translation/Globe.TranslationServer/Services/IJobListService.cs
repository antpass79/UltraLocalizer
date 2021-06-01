using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IJobListService : IAsyncService<JobListConcept, NewJobList, int>
    {
        public Task<IEnumerable<JobListConcept>> GetAllAsync(string componentNamespace, string internalNamespace, int languageId, int jobListId);
        Task SaveAsync(NewJobList newJobList);
    }
}
