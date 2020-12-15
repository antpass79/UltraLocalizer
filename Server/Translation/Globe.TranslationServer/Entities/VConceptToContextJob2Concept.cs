using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class VConceptToContextJob2Concept
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string LocalizationId { get; set; }
        public string ContextName { get; set; }
        public bool? Ignore { get; set; }
        public int Idconcept { get; set; }
        public int Idcontext { get; set; }
        public int Id { get; set; }
        public string Comment { get; set; }
    }
}
