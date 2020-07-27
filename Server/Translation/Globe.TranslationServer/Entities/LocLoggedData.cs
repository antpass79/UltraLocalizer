using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class LocLoggedData
    {
        public int Id { get; set; }
        public int SessionDataId { get; set; }
        public string LoggedString { get; set; }

        public virtual LocSessionData SessionData { get; set; }
    }
}
