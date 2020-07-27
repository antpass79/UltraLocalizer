using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class LocJobList
    {
        public LocJobList()
        {
            LocJob2Concept = new HashSet<LocJob2Concept>();
        }

        public int Id { get; set; }
        public string JobName { get; set; }
        public string UserName { get; set; }
        public int IdisoCoding { get; set; }

        public virtual LocLanguages IdisoCodingNavigation { get; set; }
        public virtual ICollection<LocJob2Concept> LocJob2Concept { get; set; }
    }
}
