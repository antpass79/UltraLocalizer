using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class LocJob2Concept
    {
        public int Id { get; set; }
        public int IdjobList { get; set; }
        public int Idconcept2Context { get; set; }

        public virtual LocConcept2Context Idconcept2ContextNavigation { get; set; }
        public virtual LocJobList IdjobListNavigation { get; set; }
    }
}
