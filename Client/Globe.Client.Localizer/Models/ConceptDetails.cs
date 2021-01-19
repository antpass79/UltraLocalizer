using System.Collections.Generic;

namespace Globe.Client.Localizer.Models
{
    class ConceptDetails
    {
        public string SoftwareDeveloperComment { get; set; }
        public string MasterTranslatorComment { get; set; }
        public bool IgnoreTranslation { get; set; }
        public IEnumerable<OriginalStringContextValue> OriginalStringContextValues { get; set; }
    }

    class OriginalStringContextValue
    {
        public string ContextName { get; set; }
        public string StringValue { get; set; }
    }
}
