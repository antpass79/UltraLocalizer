using Globe.Shared.DTOs;
using Globe.Shared.Utilities;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.XmlManager;
using Globe.TranslationServer.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Globe.TranslationServer.Services.NewServices
{
    class ScopedLocalizationResourceBuilder : ILocalizationResourceBuilder
    {
        #region Data Members

        private readonly DbContextOptions<LocalizationContext> _dbContextOptions;

        ComponentNamespaceGroup<ComponentNamespace, InternalNamespace> _componentNamespaceGroup;
        Language _language;
        bool _debugMode = true;

        #endregion

        #region Constructors

        public ScopedLocalizationResourceBuilder(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LocalizationContext>();
            var connectionString = configuration.GetConnectionString(Constants.DEFAULT_CONNECTION_STRING);
            optionsBuilder.UseSqlServer(connectionString);
            _dbContextOptions = optionsBuilder.Options;
        }

        #endregion

        #region Public Functions

        public ILocalizationResourceBuilder ComponentNamespaceGroup(ComponentNamespaceGroup<ComponentNamespace,InternalNamespace> componentNamespaceGroup)
        {
            _componentNamespaceGroup = componentNamespaceGroup;
            return this;
        }

        public ILocalizationResourceBuilder Language(Language language)
        {
            _language = language;
            return this;
        }

        public ILocalizationResourceBuilder DebugMode(bool debugMode)
        {
            _debugMode = debugMode;
            return this;
        }

        public LocalizationResource Build()
        {
            LocalizationResource localizationResource = new LocalizationResource
            {
                ComponentNamespace = _componentNamespaceGroup.ComponentNamespace.Description,
                Language = _language.IsoCoding,
                Version = (decimal)1.0
            };

            IEnumerable<VTranslatedConcept> translatedConcepts = GetTranslatedConcepts(_componentNamespaceGroup, _language);
            
            if(_language.IsoCoding != SharedConstants.LANGUAGE_EN)
            {
                IEnumerable<VStringsToContext> englishTranslatedConcepts = GetEnglishTranslatedConceptsForOtherLanguage(_componentNamespaceGroup.ComponentNamespace.Description, _language.IsoCoding);

                if(englishTranslatedConcepts.Count() > 0)
                {
                    translatedConcepts = translatedConcepts.Union(englishTranslatedConcepts.Select(item => new VTranslatedConcept
                                    {
                                        Concept = item.LocalizationId,
                                        ConceptComponentNamespace = item.ComponentNamespace,
                                        ConceptInternalNamespace = item.InternalNamespace,
                                        Context = item.ContextName,
                                        LanguageIsoCode = _language.IsoCoding,
                                        String = item.String
                                    }));
                }                         
            }
            
            var internalNamespaceSectionGroups = from translatedConcept in translatedConcepts
                                                 group translatedConcept by translatedConcept.ConceptInternalNamespace;

            foreach (IGrouping<string, VTranslatedConcept> internalNamespaceSectionGroup in internalNamespaceSectionGroups)
            {
                LocalizationSection localizationSection = new LocalizationSection
                {
                    InternalNamespace = internalNamespaceSectionGroup.Key == SharedConstants.INTERNAL_NAMESPACE_NULL ? null : internalNamespaceSectionGroup.Key
                };
                localizationResource.LocalizationSection.Add(localizationSection);

                var contextSectionGroups = from section in internalNamespaceSectionGroup
                                           group section by section.Concept;

                foreach (IGrouping<string, VTranslatedConcept> contextSectionGroup in contextSectionGroups)
                {
                    var concept = BuildConcept(contextSectionGroup, _debugMode);
                    localizationSection.Concept.Add(concept);
                }
            }

            return localizationResource;
        }

        #endregion

        #region Private Functions

        Concept BuildConcept(IGrouping<string, VTranslatedConcept> contextSectionGroup, bool debugMode)
        {
            Concept concept = new Concept
            {
                Id = contextSectionGroup.Key
            };

            foreach (VTranslatedConcept context in contextSectionGroup)
            {
                TagString tagString = new TagString();
                tagString.Context = context.Context;
                tagString.TypedValue = context.String;
                if (debugMode)
                {
                    tagString.DatabaseID = context.StringId;
                    //tagString.IsAcceptable = context.IsAcceptable;
                }
                concept.String.Add(tagString);
            }

            return concept;
        }

        IEnumerable<VTranslatedConcept> GetTranslatedConcepts(ComponentNamespaceGroup<ComponentNamespace, InternalNamespace> componentNamespaceGroup, Language language)
        {
            using var localContext = new LocalizationContext(_dbContextOptions);
            IEnumerable<VTranslatedConcept> translatedConcepts = new List<VTranslatedConcept>();

            if (language.IsoCoding == SharedConstants.LANGUAGE_EN)
            {
                foreach(InternalNamespace internalNamespace in componentNamespaceGroup.InternalNamespaces)
                {
                    translatedConcepts = translatedConcepts.Union(localContext.VTranslatedConcepts
                    .Where(item =>
                        item.ConceptComponentNamespace == componentNamespaceGroup.ComponentNamespace.Description &&
                        item.ConceptInternalNamespace == internalNamespace.Description && 
                        item.LanguageIsoCode == SharedConstants.LANGUAGE_EN));
                }
            }
            else
            {
                foreach (InternalNamespace internalNamespace in componentNamespaceGroup.InternalNamespaces)
                {
                    translatedConcepts = translatedConcepts.Union(localContext.VTranslatedConcepts
                    .Where(item =>
                        item.ConceptComponentNamespace == componentNamespaceGroup.ComponentNamespace.Description &&
                        item.ConceptInternalNamespace == internalNamespace.Description &&
                        item.LanguageIsoCode == language.IsoCoding &&
                        (!item.ConceptIgnore.HasValue || !item.ConceptIgnore.Value))
                    .Union(localContext.VTranslatedConcepts
                            .Where(item =>
                                item.ConceptComponentNamespace == componentNamespaceGroup.ComponentNamespace.Description &&
                                item.ConceptInternalNamespace == internalNamespace.Description &&
                                item.LanguageIsoCode == SharedConstants.LANGUAGE_EN &&
                                item.ConceptIgnore.HasValue && item.ConceptIgnore.Value)
                            ));
                }
            }

            return translatedConcepts.ToList();

            // ASK TO LAURA BIGI
            // Replace all localized strings
            //if (ComponentName == "MeasureComponent")
            //{
            //    var z = from k in englishConcepts
            //            where k.ContextName == "MD_RT11ExtLabel"
            //            select k;
            //    foreach (var rz in z)
            //    {
            //        DBGlobal item = retList.FirstOrDefault(t => t.ComponentNamespace == rz.ComponentNamespace &&
            //                                                    t.InternalNamespace == rz.InternalNamespace &&
            //                                                    t.LocalizationID == rz.LocalizationID &&
            //                                                    t.ContextName == rz.ContextName);
            //        var item2 = specificLanguageConcepts.FirstOrDefault(t => t.ComponentNamespace == rz.ComponentNamespace &&
            //                                                    t.InternalNamespace == rz.InternalNamespace &&
            //                                                    t.LocalizationID == rz.LocalizationID &&
            //                                                    t.ContextName == rz.ContextName);
            //        if (item != null && item2 == null)
            //        {
            //            var itemFallback = specificLanguageConcepts.FirstOrDefault(t => t.ComponentNamespace == rz.ComponentNamespace &&
            //                                                        t.InternalNamespace == rz.InternalNamespace &&
            //                                                        t.LocalizationID == rz.LocalizationID &&
            //                                                        t.ContextName == "MD_RT11MeasName");
            //            if (itemFallback != null)
            //            {
            //                item.DataString = itemFallback.String;
            //                item.DatabaseID = itemFallback.StringID;
            //                item.IsAcceptable = itemFallback.IsAcceptable;
            //                item.Concept2ContextID = itemFallback.ID;
            //            }
            //        }
            //    }
            //}
        }

        IEnumerable<VStringsToContext> GetEnglishTranslatedConceptsForOtherLanguage(string conceptComponentNamespace, string isocoding)
        {
            using var localContext = new LocalizationContext(_dbContextOptions);

            var subQuery = localContext.VStringsToContexts
                .Where(item =>
                    item.Isocoding == isocoding &&
                    item.ComponentNamespace != SharedConstants.COMPONENT_NAMESPACE_OLD &&
                    item.ComponentNamespace == conceptComponentNamespace)
                .Select(item => item.Id);

            var items = localContext.VStringsToContexts
                .Where(item =>
                    item.Ignore.HasValue &&
                    !item.Ignore.Value &&
                    item.Isocoding == SharedConstants.LANGUAGE_EN &&
                    item.ComponentNamespace != SharedConstants.COMPONENT_NAMESPACE_OLD &&
                    item.ComponentNamespace == conceptComponentNamespace &&
                    !subQuery.Contains(item.Id))
                .AsNoTracking()
                .ToList();

            return items;
        }

        #endregion
    }
}
