using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class ConceptDetailsAdapterService : IAsyncConceptDetailsService
    {
        private readonly IXmlToDBService _xmlService;
        private readonly IAsyncConceptService _conceptService;

        public ConceptDetailsAdapterService(IXmlToDBService xmlService, IAsyncConceptService conceptService)
        {
            _xmlService = xmlService;
            _conceptService = conceptService;
        }

        async public Task<ConceptDetailsDTO> GetAsync(JobListConcept jobListConcept)
        {         
            var softwareDeveloperComment = _xmlService.GetSoftwareDeveloperComment(jobListConcept.ComponentNamespace, jobListConcept.InternalNamespace, jobListConcept.Name);
            var currentConcept = await _conceptService.GetConceptAsync(jobListConcept.Id);
            var masterTranslatorComment = currentConcept.Comment;
            var ignore = currentConcept.Ignore;

            var conceptDetails = new ConceptDetailsDTO
            {
                SoftwareDeveloperComment = softwareDeveloperComment,
                MasterTranslatorComment = masterTranslatorComment,
                IgnoreTranslation = ignore,
                OriginalStringContextValues = jobListConcept.ContextViews.Select(item => new OriginalStringContextValueDTO { ContextName = item.Name, StringValue = _xmlService.GetOriginalDeveloperString(jobListConcept.ComponentNamespace, jobListConcept.InternalNamespace == "null" ? null : jobListConcept.InternalNamespace, jobListConcept.Name, item.Name) })
            };

            return await Task.FromResult(conceptDetails);
        }
    }
}
