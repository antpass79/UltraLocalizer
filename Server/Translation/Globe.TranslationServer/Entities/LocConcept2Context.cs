using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class LocConcept2Context
    {
        public LocConcept2Context()
        {
            LocJob2Concept = new HashSet<LocJob2Concept>();
            LocStrings2Context = new HashSet<LocStrings2Context>();
        }

        public int Id { get; set; }
        public int Idconcept { get; set; }
        public int Idcontext { get; set; }

        public virtual LocConceptsTable IdconceptNavigation { get; set; }
        public virtual LocContexts IdcontextNavigation { get; set; }
        public virtual ICollection<LocJob2Concept> LocJob2Concept { get; set; }
        public virtual ICollection<LocStrings2Context> LocStrings2Context { get; set; }
    }
}
