using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class LocStrings
    {
        public LocStrings()
        {
            LocStrings2Context = new HashSet<LocStrings2Context>();
            LocStrings2translate = new HashSet<LocStrings2translate>();
            LocStringslocked = new HashSet<LocStringslocked>();
        }

        public int Id { get; set; }
        public int Idlanguage { get; set; }
        public int Idtype { get; set; }
        public string String { get; set; }

        public virtual LocLanguages IdlanguageNavigation { get; set; }
        public virtual LocStringTypes IdtypeNavigation { get; set; }
        public virtual ICollection<LocStrings2Context> LocStrings2Context { get; set; }
        public virtual ICollection<LocStrings2translate> LocStrings2translate { get; set; }
        public virtual ICollection<LocStringslocked> LocStringslocked { get; set; }
    }
}
