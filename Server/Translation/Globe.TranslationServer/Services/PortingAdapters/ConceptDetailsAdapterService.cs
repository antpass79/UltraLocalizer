using Globe.Shared.DTOs;
using Globe.Shared.Services;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept;
using Globe.TranslationServer.Porting.UltraDBDLL.XmlManager;
using Globe.TranslationServer.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class ConceptDetailsAdapterService : IAsyncConceptDetailsService
    {
        private readonly XmlManager _xmlManager;
        private readonly UltraDBConcept _ultraDBConcept;
        private readonly ILogService _logService;

        public ConceptDetailsAdapterService(XmlManager xmlManager, UltraDBConcept ultraDBConcept, ILogService logService)
        {
            _xmlManager = xmlManager;
            _xmlManager.XmlDirectory = Constants.XML_FOLDER;
            _xmlManager.LoadXmlOnly();
            _ultraDBConcept = ultraDBConcept;
            _logService = logService;
        }

        async public Task<ConceptDetailsDTO> GetAsync(JobListConcept jobListConcept)
        {
            var key = _xmlManager.GetKey(jobListConcept.ComponentNamespace, jobListConcept.InternalNamespace, jobListConcept.Name);
            string softwareDeveloperComment = string.Empty;
            try
            {
                softwareDeveloperComment = _xmlManager.KeyComments[key];
            }
            catch (Exception e)
            {
                _logService.Exception(e);
                softwareDeveloperComment = "No comment Found";               
            }
           
            var currentConcept = _ultraDBConcept.GetConceptbyID(jobListConcept.Id);
            var masterTranslatorComment = currentConcept.Comment;
            var ignore = currentConcept.Ignore;

            var conceptDetails = new ConceptDetailsDTO
            {
                SoftwareDeveloperComment = softwareDeveloperComment,
                MasterTranslatorComment = masterTranslatorComment,
                IgnoreTranslation = ignore,
                OriginalStringContextValues = jobListConcept.ContextViews.Select(item => new OriginalStringContextValueDTO { ContextName = item.Name, StringValue = _xmlManager.GetUserString(jobListConcept.ComponentNamespace, jobListConcept.InternalNamespace == "null" ? null : jobListConcept.InternalNamespace, jobListConcept.Name, item.Name) })
            };

            return await Task.FromResult(conceptDetails);
        }
    }
}
