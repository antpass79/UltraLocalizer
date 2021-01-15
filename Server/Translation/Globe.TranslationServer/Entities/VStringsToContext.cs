using System;
using System.Collections.Generic;

#nullable disable

namespace Globe.TranslationServer.Entities
{
    public partial class VStringsToContext
    {
        public int Id { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string LocalizationId { get; set; }
        public string Isocoding { get; set; }
        public string ContextName { get; set; }
        public string String { get; set; }
        public int StringId { get; set; }
        public int Idtype { get; set; }
        public string Type { get; set; }
        public bool? Ignore { get; set; }
        public int ConceptId { get; set; }
    }
}
