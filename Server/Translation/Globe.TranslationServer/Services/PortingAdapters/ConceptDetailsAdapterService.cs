using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept;
using Globe.TranslationServer.Porting.UltraDBDLL.XmlManager;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class ConceptDetailsAdapterService : IAsyncConceptDetailsService
    {
        const string XML_FOLDER = "XmlDefinitions";

        private readonly XmlManager _xmlManager;
        private readonly UltraDBConcept _ultraDBConcept;

        public ConceptDetailsAdapterService(XmlManager xmlManager, UltraDBConcept ultraDBConcept)
        {
            _xmlManager = xmlManager;
            _xmlManager.XmlDirectory = XML_FOLDER;
            _xmlManager.LoadXmlOnly();
            _ultraDBConcept = ultraDBConcept;
        }

        async public Task<ConceptDetailsDTO> GetAsync(ConceptViewDTO concept)
        {
            var key = _xmlManager.GetKey(concept.ComponentNamespace, concept.InternalNamespace, concept.Name);
            var softwareDeveloperComment = _xmlManager.KeyComments[key];

            var currentConcept = _ultraDBConcept.GetConceptbyID(concept.Id);
            var masterTranslatorComment = currentConcept.Comment;

            var conceptDetails = new ConceptDetailsDTO
            {
                SoftwareDeveloperComment = softwareDeveloperComment,
                MasterTranslatorComment = masterTranslatorComment,
                OriginalStringContextValues = concept.ContextViews.Select(item => new OriginalStringContextValueDTO { ContextName = item.Name, StringValue = _xmlManager.GetUserString(concept.ComponentNamespace, concept.InternalNamespace == "null" ? null : concept.InternalNamespace, concept.Name, item.Name) })
            };

            return await Task.FromResult(conceptDetails);
        }
    }
}
