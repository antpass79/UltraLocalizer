using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class LocStringslocked
    {
        public int Id { get; set; }
        public int Idstring { get; set; }

        public virtual LocStrings IdstringNavigation { get; set; }
    }
}
