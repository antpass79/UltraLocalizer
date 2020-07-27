using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class LocConceptsTable
    {
        public LocConceptsTable()
        {
            LocConcept2Context = new HashSet<LocConcept2Context>();
        }

        public int Id { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string LocalizationId { get; set; }
        public bool? Ignore { get; set; }
        public string Comment { get; set; }
        public byte[] SsmaTimeStamp { get; set; }

        public virtual ICollection<LocConcept2Context> LocConcept2Context { get; set; }
    }
}
