using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class VJobToConcept
    {
        public int Id { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string LocalizationId { get; set; }
        public string Comment { get; set; }
        public int Idconcept2Context { get; set; }
        public string ContextName { get; set; }
        public int Idcontext { get; set; }
        public int Idstring2Context { get; set; }
        public string String { get; set; }
        public int Idlanguage { get; set; }
        public int Idtype { get; set; }
        public string StringType { get; set; }
        public int Idstring { get; set; }
    }
}
