using System;
using System.Collections.Generic;

#nullable disable

namespace MyLabLocalizer.LocalizationService.Entities
{
    public partial class VString
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public int StringTypeId { get; set; }
        public string String { get; set; }
        public int ConceptToContextId { get; set; }
        public int StringToContextId { get; set; }
    }
}
