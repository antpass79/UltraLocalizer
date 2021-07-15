using System;
using System.Collections.Generic;

#nullable disable

namespace MyLabLocalizer.LocalizationService.Entities
{
    public partial class VTranslatedConcept
    {
        public int ConceptId { get; set; }
        public string Concept { get; set; }
        public string ConceptComponentNamespace { get; set; }
        public string ConceptInternalNamespace { get; set; }
        public string ConceptComment { get; set; }
        public bool? ConceptIgnore { get; set; }
        public int ContextId { get; set; }
        public string Context { get; set; }
        public int ConceptToContextId { get; set; }
        public int StringId { get; set; }
        public string String { get; set; }
        public int StringTypeId { get; set; }
        public string StringType { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public string LanguageIsoCode { get; set; }
    }
}
