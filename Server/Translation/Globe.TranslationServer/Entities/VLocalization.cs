﻿using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class VLocalization
    {
        public int ConceptId { get; set; }
        public string Concept { get; set; }
        public string ConceptComponentNamespace { get; set; }
        public string ConceptInternalNamespace { get; set; }
        public string ConceptComment { get; set; }
        public int? ContextId { get; set; }
        public string Context { get; set; }
        public int? StringId { get; set; }
        public string String { get; set; }
        public int? StringTypeId { get; set; }
        public string StringType { get; set; }
        public int? LanguageId { get; set; }
        public string Language { get; set; }
        public string LanguageIsoCode { get; set; }
        public int? JobListId { get; set; }
        public string JobList { get; set; }
        public string JobListUserName { get; set; }
        public int IsAcceptable { get; set; }
    }
}