using System;
using System.Collections.Generic;

#nullable disable

namespace MyLabLocalizer.LocalizationService.Entities
{
    public partial class LocConceptsTable
    {
        public LocConceptsTable()
        {
            LocConcept2Contexts = new HashSet<LocConcept2Context>();
        }

        public int Id { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string LocalizationId { get; set; }
        public bool? Ignore { get; set; }
        public string Comment { get; set; }
        public byte[] SsmaTimeStamp { get; set; }

        public virtual ICollection<LocConcept2Context> LocConcept2Contexts { get; set; }
    }
}
