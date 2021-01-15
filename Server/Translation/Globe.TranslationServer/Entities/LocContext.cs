using System;
using System.Collections.Generic;

#nullable disable

namespace Globe.TranslationServer.Entities
{
    public partial class LocContext
    {
        public LocContext()
        {
            LocConcept2Contexts = new HashSet<LocConcept2Context>();
        }

        public int Id { get; set; }
        public string ContextName { get; set; }

        public virtual ICollection<LocConcept2Context> LocConcept2Contexts { get; set; }
    }
}
