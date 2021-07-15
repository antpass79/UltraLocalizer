using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.DTOs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyLabLocalizer.LocalizationService.Services
{
    public class XmlDefinitionReaderService : IAsyncXmlDefinitionReaderService
    {
        const string ATTRIBUTE_COMPONENT_NAMESPACE = "ComponentNamespace";
        const string ATTRIBUTE_LANGUAGE = "Language";
        const string ATTRIBUTE_VERSION = "Version";

        const string ATTRIBUTE_INTERNAL_NAMESPACE = "InternalNamespace";
        const string ATTRIBUTE_CONCEPT_ID = "Id";

        const string ATTRIBUTE_CONTEXT = "Context";

        const string TAG_LOCALIZATION_SECTION = "LocalizationSection";
        const string TAG_CONCEPT = "Concept";
        const string TAG_COMMENTS = "Comments";
        const string TAG_STRING = "String";

        async public Task<IEnumerable<JobListConcept>> ReadAsync(string folder)
        {
            var jobListConcepts = new List<JobListConcept>();

            IEnumerable<string> filePaths = Directory.EnumerateFiles(folder, "*.definition.xml").Select(fileName => Path.Combine(folder, fileName));
            foreach (var filePath in filePaths)
            {
                XDocument document = await XDocument.LoadAsync(File.OpenText(filePath), LoadOptions.PreserveWhitespace, new System.Threading.CancellationToken());
                var componentNamespace = document.Root.Attribute(ATTRIBUTE_COMPONENT_NAMESPACE);

                var localizationSectionTags = document.Descendants(TAG_LOCALIZATION_SECTION);

                foreach (var localizationSectionTag in localizationSectionTags)
                {
                    var internalNamespace = localizationSectionTag.Attribute(ATTRIBUTE_INTERNAL_NAMESPACE);

                    var conceptTags = localizationSectionTag.Descendants(TAG_CONCEPT);

                    foreach (var conceptTag in conceptTags)
                    {
                        var conceptId = conceptTag.Attribute(ATTRIBUTE_CONCEPT_ID);

                        var jobListConcept = new JobListConcept
                        {
                            ComponentNamespace = componentNamespace != null ? componentNamespace.Value : string.Empty,
                            InternalNamespace = internalNamespace != null ? internalNamespace.Value : string.Empty,
                            Name = conceptId != null ? conceptId.Value : string.Empty,
                            ContextViews = new List<JobListContext>()
                        };

                        jobListConcepts.Add(jobListConcept);

                        var contextTags = conceptTag.Descendants(TAG_STRING);
                        foreach (var contextTag in contextTags)
                        {
                            var context = contextTag.Attribute(ATTRIBUTE_CONTEXT);

                            jobListConcept.ContextViews.Add(new JobListContext
                            {
                                Name = context.Value,
                                StringValue = contextTag.Value
                            });
                        }
                    }
                }
            }

            return await Task.FromResult(jobListConcepts);
        }
    }
}
