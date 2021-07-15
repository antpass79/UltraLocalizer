using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Models
{
    public class LocalizationSectionSubset
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string ConceptId { get; set; }
        public string DeveloperComment { get; set; }
        public IEnumerable<string> Contexts { get; set; }
        public string FileName { get; set; }
    }
}
