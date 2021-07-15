using System;
using System.Collections.Generic;

#nullable disable

namespace MyLabLocalizer.LocalizationService.Entities
{
    public partial class LocLanguage
    {
        public LocLanguage()
        {
            LocJobLists = new HashSet<LocJobList>();
            LocStrings = new HashSet<LocString>();
        }

        public int Id { get; set; }
        public string LanguageName { get; set; }
        public string Isocoding { get; set; }

        public virtual ICollection<LocJobList> LocJobLists { get; set; }
        public virtual ICollection<LocString> LocStrings { get; set; }
    }
}
