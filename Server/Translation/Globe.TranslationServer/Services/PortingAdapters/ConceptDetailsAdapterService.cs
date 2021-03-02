﻿using Globe.Shared.DTOs;
using Globe.Shared.Services;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class ConceptDetailsAdapterService : IAsyncConceptDetailsService
    {
        private readonly IXmlService _xmlService;
        private readonly UltraDBConcept _ultraDBConcept;
        private readonly ILogService _logService;

        public ConceptDetailsAdapterService(IXmlService xmlService, UltraDBConcept ultraDBConcept, ILogService logService)
        {
            _xmlService = xmlService;
            _xmlService.LoadXml();
            _ultraDBConcept = ultraDBConcept;
            _logService = logService;
        }

        async public Task<ConceptDetailsDTO> GetAsync(JobListConcept jobListConcept)
        {         
            var softwareDeveloperComment = _xmlService.GetSoftwareDeveloperComment(jobListConcept.ComponentNamespace, jobListConcept.InternalNamespace, jobListConcept.Name);
            var currentConcept = _ultraDBConcept.GetConceptbyID(jobListConcept.Id);
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
