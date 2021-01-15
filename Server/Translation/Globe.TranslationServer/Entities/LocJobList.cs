using System;
using System.Collections.Generic;

#nullable disable

namespace Globe.TranslationServer.Entities
{
    public partial class LocJobList
    {
        public LocJobList()
        {
            LocJob2Concepts = new HashSet<LocJob2Concept>();
        }

        public int Id { get; set; }
        public string JobName { get; set; }
        public string UserName { get; set; }
        public int IdisoCoding { get; set; }

        public virtual LocLanguage IdisoCodingNavigation { get; set; }
        public virtual ICollection<LocJob2Concept> LocJob2Concepts { get; set; }
    }
}
