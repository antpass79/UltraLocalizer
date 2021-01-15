using System;
using System.Collections.Generic;

#nullable disable

namespace Globe.TranslationServer.Entities
{
    public partial class LocConcept2Context
    {
        public LocConcept2Context()
        {
            LocJob2Concepts = new HashSet<LocJob2Concept>();
            LocStrings2Contexts = new HashSet<LocStrings2Context>();
        }

        public int Id { get; set; }
        public int Idconcept { get; set; }
        public int Idcontext { get; set; }

        public virtual LocConceptsTable IdconceptNavigation { get; set; }
        public virtual LocContext IdcontextNavigation { get; set; }
        public virtual ICollection<LocJob2Concept> LocJob2Concepts { get; set; }
        public virtual ICollection<LocStrings2Context> LocStrings2Contexts { get; set; }
    }
}
