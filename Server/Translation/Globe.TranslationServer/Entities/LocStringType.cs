using System;
using System.Collections.Generic;

#nullable disable

namespace Globe.TranslationServer.Entities
{
    public partial class LocStringType
    {
        public LocStringType()
        {
            LocStrings = new HashSet<LocString>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<LocString> LocStrings { get; set; }
    }
}
