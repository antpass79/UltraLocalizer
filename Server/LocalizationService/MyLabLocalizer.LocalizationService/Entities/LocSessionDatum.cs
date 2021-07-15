using System;
using System.Collections.Generic;

#nullable disable

namespace MyLabLocalizer.LocalizationService.Entities
{
    public partial class LocSessionDatum
    {
        public LocSessionDatum()
        {
            LocLoggedData = new HashSet<LocLoggedDatum>();
        }

        public int Id { get; set; }
        public string SessionId { get; set; }
        public string UserName { get; set; }
        public DateTime InitSessionDate { get; set; }
        public DateTime LastModify { get; set; }

        public virtual ICollection<LocLoggedDatum> LocLoggedData { get; set; }
    }
}
