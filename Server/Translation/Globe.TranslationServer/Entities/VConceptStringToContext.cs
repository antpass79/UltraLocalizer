using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class VConceptStringToContext
    {
        public int Id { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string LocalizationId { get; set; }
        public int Idcontext { get; set; }
        public int Idconcept2Context { get; set; }
        public string ContextName { get; set; }
        public int? Idstring { get; set; }
    }
}
