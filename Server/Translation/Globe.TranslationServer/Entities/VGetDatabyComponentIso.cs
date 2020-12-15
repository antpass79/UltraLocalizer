using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class VGetDatabyComponentIso
    {
        public int Id { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string LocalizationId { get; set; }
        public string Isocoding { get; set; }
        public string ContextName { get; set; }
        public string String { get; set; }
        public int Idtype { get; set; }
        public string Type { get; set; }
        public int StringId { get; set; }
        public bool? Ignore { get; set; }
        public int ConceptId { get; set; }
        public int IsAcceptable { get; set; }
    }
}
