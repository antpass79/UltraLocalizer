using Globe.BusinessLogic.Repositories;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings;
using Globe.TranslationServer.Porting.UltraDBDLL.XmlManager;
using Globe.TranslationServer.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class DBToXmlService : IDBToXmlService
    {
        private readonly IAsyncReadRepository<VLocalization> _localizationViewRepository;
        IAsyncReadRepository<LocLanguages> _languageRepository;
        private readonly UltraDBGlobal _ultraDBGlobal;

        public DBToXmlService(
            IAsyncReadRepository<VLocalization> localizationViewRepository,
            IAsyncReadRepository<LocLanguages> languageRepository,
            UltraDBGlobal ultraDBGlobal)
        {
            _localizationViewRepository = localizationViewRepository;
            _languageRepository = languageRepository;
            _ultraDBGlobal = ultraDBGlobal;
        }

        async public Task Generate(string outputFolder, bool debugMode = true)
        {
            var components = (await _localizationViewRepository
                .QueryAsync())
                .ToList()
                .GroupBy(item => item.ConceptComponentNamespace)
                .Select(item => item.First())
                .OrderBy(item => item.ConceptComponentNamespace);

            var languages = (await _languageRepository.QueryAsync())
                .ToList();

            Parallel.ForEach(components, (component) =>
            {
                if (
                component.ConceptComponentNamespace != Constants.COMPONENT_NAMESPACE_ALL &&
                component.ConceptComponentNamespace != Constants.COMPONENT_NAMESPACE_OLD)
                {
                    Parallel.ForEach(languages, (language) =>
                    {
                        LocalizationResource localizationResource = new LocalizationResource
                        {
                            ComponentNamespace = component.ConceptComponentNamespace,
                            Language = language.Isocoding,
                            Version = (decimal)1.0
                        };

                        List<DBGlobal> xmlFiles = _ultraDBGlobal.GetDataByComponentISO(component.ConceptComponentNamespace, language.ToString());
                        var sections = from xmlFile in xmlFiles
                                       group new DBGlobal
                                       {
                                           LocalizationID = xmlFile.LocalizationID,
                                           ContextName = xmlFile.ContextName,
                                           DataString = xmlFile.DataString,
                                           DatabaseID = xmlFile.DatabaseID,
                                           IsAcceptable = xmlFile.IsAcceptable
                                       }
                                       by xmlFile.InternalNamespace;

                        foreach (IGrouping<string, DBGlobal> sectionGroup in sections)
                        {
                            LocalizationSection localizationSection = new LocalizationSection
                            {
                                InternalNamespace = sectionGroup.Key == Constants.INTERNAL_NAMESPACE_NULL ? null : sectionGroup.Key
                            };
                            localizationResource.LocalizationSection.Add(localizationSection);

                            var concepts = from section in sectionGroup
                                           group new DBGlobal
                                           {
                                               LocalizationID = section.LocalizationID,
                                               ContextName = section.ContextName,
                                               DataString = section.DataString,
                                               DatabaseID = section.DatabaseID,
                                               IsAcceptable = section.IsAcceptable
                                           }
                                           by section.LocalizationID;

                            foreach (IGrouping<string, DBGlobal> conceptGroup in concepts)
                            {
                                Concept concept = new Concept
                                {
                                    Id = conceptGroup.Key
                                };
                                localizationSection.Concept.Add(concept);

                                foreach (DBGlobal global in conceptGroup)
                                {
                                    TagString tagString = new TagString();
                                    tagString.Context = global.ContextName;
                                    tagString.TypedValue = global.DataString;
                                    if (debugMode)
                                    {
                                        tagString.DatabaseID = global.DatabaseID;
                                        tagString.IsAcceptable = global.IsAcceptable;
                                    }
                                    concept.String.Add(tagString);
                                }
                            }
                        }
                        localizationResource.Save(Path.Combine(outputFolder, $"{localizationResource.ComponentNamespace}.{localizationResource.Language}.xml"));
                    });
                }
            });

            await Task.CompletedTask;
        }
    }
}
