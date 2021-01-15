using System;
using System.Collections.Generic;

#nullable disable

namespace Globe.TranslationServer.Entities
{
    public partial class LocStrings2translate
    {
        public int Id { get; set; }
        public int Idstring { get; set; }

        public virtual LocString IdstringNavigation { get; set; }
    }
}
