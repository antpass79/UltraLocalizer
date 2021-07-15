using System.Collections.Generic;

namespace MyLabLocalizer.Shared.DTOs
{
    public class LocalizeString
    {
        public int StringId { get; set; }
        public string Value { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public int StringTypeId { get; set; }
        public string StringType { get; set; }
        public IList<LocalizeStringDetail> LocalizeStringDetails { get; set; }
    }
}
