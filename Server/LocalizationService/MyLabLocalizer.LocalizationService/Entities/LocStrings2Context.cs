using System;
using System.Collections.Generic;

#nullable disable

namespace MyLabLocalizer.LocalizationService.Entities
{
    public partial class LocStrings2Context
    {
        public int Id { get; set; }
        public int Idstring { get; set; }
        public int Idconcept2Context { get; set; }

        public virtual LocConcept2Context Idconcept2ContextNavigation { get; set; }
        public virtual LocString IdstringNavigation { get; set; }
    }
}
