using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept;
using Globe.TranslationServer.Porting.UltraDBDLL.XmlManager;
using Globe.TranslationServer.Utilities;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class ConceptDetailsAdapterService : IAsyncConceptDetailsService
    {
        private readonly XmlManager _xmlManager;
        private readonly UltraDBConcept _ultraDBConcept;

        public ConceptDetailsAdapterService(XmlManager xmlManager, UltraDBConcept ultraDBConcept)
        {
            _xmlManager = xmlManager;
            _xmlManager.XmlDirectory = Constants.XML_FOLDER;
            _xmlManager.LoadXmlOnly();
            _ultraDBConcept = ultraDBConcept;
        }

        async public Task<ConceptDetailsDTO> GetAsync(JobListConcept jobListConcept)
        {
            var key = _xmlManager.GetKey(jobListConcept.ComponentNamespace, jobListConcept.InternalNamespace, jobListConcept.Name);
            var softwareDeveloperComment = _xmlManager.KeyComments[key];

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
