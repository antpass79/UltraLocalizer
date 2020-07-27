using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class LocContexts
    {
        public LocContexts()
        {
            LocConcept2Context = new HashSet<LocConcept2Context>();
        }

        public int Id { get; set; }
        public string ContextName { get; set; }

        public virtual ICollection<LocConcept2Context> LocConcept2Context { get; set; }
    }
}
