using System;
using System.Collections.Generic;

#nullable disable

namespace Globe.TranslationServer.Entities
{
    public partial class LocString
    {
        public LocString()
        {
            LocStrings2Contexts = new HashSet<LocStrings2Context>();
            LocStrings2translates = new HashSet<LocStrings2translate>();
            LocStringslockeds = new HashSet<LocStringslocked>();
        }

        public int Id { get; set; }
        public int Idlanguage { get; set; }
        public int Idtype { get; set; }
        public string String { get; set; }

        public virtual LocLanguage IdlanguageNavigation { get; set; }
        public virtual LocStringType IdtypeNavigation { get; set; }
        public virtual ICollection<LocStrings2Context> LocStrings2Contexts { get; set; }
        public virtual ICollection<LocStrings2translate> LocStrings2translates { get; set; }
        public virtual ICollection<LocStringslocked> LocStringslockeds { get; set; }
    }
}
