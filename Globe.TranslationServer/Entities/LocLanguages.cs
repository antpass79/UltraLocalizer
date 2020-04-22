using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class LocLanguages
    {
        public LocLanguages()
        {
            LocJobList = new HashSet<LocJobList>();
            LocStrings = new HashSet<LocStrings>();
        }

        public int Id { get; set; }
        public string LanguageName { get; set; }
        public string Isocoding { get; set; }

        public virtual ICollection<LocJobList> LocJobList { get; set; }
        public virtual ICollection<LocStrings> LocStrings { get; set; }
    }
}
