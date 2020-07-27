using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class LocStringTypes
    {
        public LocStringTypes()
        {
            LocStrings = new HashSet<LocStrings>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<LocStrings> LocStrings { get; set; }
    }
}
