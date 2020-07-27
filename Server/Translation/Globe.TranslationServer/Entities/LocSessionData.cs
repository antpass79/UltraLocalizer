using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Entities
{
    public partial class LocSessionData
    {
        public LocSessionData()
        {
            LocLoggedData = new HashSet<LocLoggedData>();
        }

        public int Id { get; set; }
        public string SessionId { get; set; }
        public string UserName { get; set; }
        public DateTime InitSessionDate { get; set; }
        public DateTime LastModify { get; set; }

        public virtual ICollection<LocLoggedData> LocLoggedData { get; set; }
    }
}
