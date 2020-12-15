using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class VStringLockedStringToTranslate
    {
        public int Id { get; set; }
        public int Idlanguage { get; set; }
        public int Idtype { get; set; }
        public string String { get; set; }
        public string Isocoding { get; set; }
        public int Idconcept2Context { get; set; }
        public int IsLocked { get; set; }
        public int Is2Translate { get; set; }
        public string ContextName { get; set; }
        public string Type { get; set; }
        public int Idcontext { get; set; }
        public int Idstrings2Context { get; set; }
    }
}
