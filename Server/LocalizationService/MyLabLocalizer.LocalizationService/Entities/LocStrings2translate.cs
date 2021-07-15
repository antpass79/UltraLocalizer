using System;
using System.Collections.Generic;

#nullable disable

namespace MyLabLocalizer.LocalizationService.Entities
{
    public partial class LocStrings2translate
    {
        public int Id { get; set; }
        public int Idstring { get; set; }

        public virtual LocString IdstringNavigation { get; set; }
    }
}
