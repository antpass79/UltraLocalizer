﻿using System.Collections.Generic;

namespace MyLabLocalizer.LocalizationService.DTOs
{
    public class ConceptDetailsDTO
    {
        public string SoftwareDeveloperComment { get; set; }
        public string MasterTranslatorComment { get; set; }
        public bool IgnoreTranslation { get; set; }
        public IEnumerable<OriginalStringContextValueDTO> OriginalStringContextValues { get; set; }
    }

    public class OriginalStringContextValueDTO
    {
        public string ContextName { get; set; }
        public string StringValue { get; set; }
    }
}
