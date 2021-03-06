﻿using System;
using System.Collections.Generic;

#nullable disable

namespace MyLabLocalizer.LocalizationService.Entities
{
    public partial class LocLoggedDatum
    {
        public int Id { get; set; }
        public int SessionDataId { get; set; }
        public string LoggedString { get; set; }

        public virtual LocSessionDatum SessionData { get; set; }
    }
}
