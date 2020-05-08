using Globe.TranslationServer.DTOs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Globe.TranslationServer.Services
{
    public class XmlDefinitionReaderService : IAsyncXmlDefinitionReaderService
    {
        const string ATTRIBUTE_COMPONENT_NAMESPACE = "ComponentNamespace";
        const string ATTRIBUTE_INTERNAL_NAMESPACE = "InternalNamespace";
        const string ATTRIBUTE_CONCEPT_ID = "Id";

        const string TAG_LOCALIZATION_SECTION = "LocalizationSection";
        const string TAG_CONCEPT = "Concept";

        async public Task<IEnumerable<StringViewItemDTO>> ReadAsync(string folder)
        {
            List<StringViewItemDTO> stringViewItems = new List<StringViewItemDTO>();

            IEnumerable<string> filePaths = Directory.EnumerateFiles(folder, "*.definition.xml").Select(fileName => Path.Combine(folder, fileName));
            foreach (var filePath in filePaths)
            {
                XDocument document = await XDocument.LoadAsync(File.OpenText(filePath), LoadOptions.PreserveWhitespace, new System.Threading.CancellationToken());
                var componentNamespace = document.Root.Attribute(ATTRIBUTE_COMPONENT_NAMESPACE);

                var localizationSections = document.Descendants(TAG_LOCALIZATION_SECTION);

                foreach (var localizationSection in localizationSections)
                {
                    var internalNamespace = localizationSection.Attribute(ATTRIBUTE_INTERNAL_NAMESPACE);

                    var concepts = localizationSection.Descendants(TAG_CONCEPT);

                    foreach (var concept in concepts)
                    {
                        var conceptId = concept.Attribute(ATTRIBUTE_CONCEPT_ID);

                        stringViewItems.Add(new StringViewItemDTO
                        {
                            ComponentNamespace = componentNamespace != null ? componentNamespace.Value : string.Empty,
                            InternalNamespace = internalNamespace != null ? internalNamespace.Value : string.Empty,
                            Concept = conceptId != null ? conceptId.Value: string.Empty
                        });
                    }
                }
            }

            return await Task.FromResult(stringViewItems);
        }
    }
}
