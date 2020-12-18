using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IJobListService : IAsyncReadService<JobListConcept>
    {
        public Task<IEnumerable<JobListConcept>> GetAllAsync(string componentNamespace, string internalNamespace, int languageId, int jobListId);
    }

    public interface IAsyncXmlGroupedStringEntityService : IAsyncReadService<JobListConcept>
    {
        public Task<IEnumerable<JobListConcept>> GetAllAsync(string componentNamespace, string internalNamespace, int languageId, int jobListId);
    }
}
