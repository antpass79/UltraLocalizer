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

        VLocalization _component;
        LocLanguage _language;
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

        public ILocalizationResourceBuilder Component(VLocalization component)
        {
            _component = component;
            return this;
        }

        public ILocalizationResourceBuilder Language(LocLanguage language)
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
                ComponentNamespace = _component.ConceptComponentNamespace,
                Language = _language.Isocoding,
                Version = (decimal)1.0
            };

            IEnumerable<VTranslatedConcept> translatedConcepts = GetTranslatedConcepts(_component.ConceptComponentNamespace, _language.Isocoding);
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

        IEnumerable<VTranslatedConcept> GetTranslatedConcepts(string componentName, string isoCoding)
        {
            using var localContext = new LocalizationContext(_dbContextOptions);
            IEnumerable<VTranslatedConcept> translatedConcepts;

            if (isoCoding == SharedConstants.LANGUAGE_EN)
            {
                translatedConcepts = localContext.VTranslatedConcepts
                    .Where(item =>
                        item.ConceptComponentNamespace == componentName &&
                        item.LanguageIsoCode == SharedConstants.LANGUAGE_EN);
            }
            else
            {
                translatedConcepts = localContext.VTranslatedConcepts
                    .Where(item =>
                        item.ConceptComponentNamespace == componentName &&
                        item.LanguageIsoCode == isoCoding &&
                        (!item.ConceptIgnore.HasValue || !item.ConceptIgnore.Value))
                    .Union(localContext.VTranslatedConcepts
                            .Where(item =>
                                item.ConceptComponentNamespace == componentName &&
                                item.LanguageIsoCode == SharedConstants.LANGUAGE_EN &&
                                item.ConceptIgnore.HasValue && item.ConceptIgnore.Value)
                            );
            }

            return translatedConcepts
                //.Select(concept => new DBGlobal
                //{
                //    ComponentNamespace = concept.ConceptComponentNamespace,
                //    ContextName = concept.Context,
                //    DataString = concept.String,
                //    InternalNamespace = concept.ConceptInternalNamespace,
                //    ISOCoding = isoCoding,
                //    LocalizationID = concept.Concept,
                //    DatabaseID = concept.StringId,
                //    //item.IsAcceptable = concept.ConceptAcceptable;
                //    Concept2ContextID = concept.ConceptId,
                //    IsToIgnore = concept.ConceptIgnore.HasValue && concept.ConceptIgnore.Value
                //})
                .ToList();


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

        #endregion
    }
}
