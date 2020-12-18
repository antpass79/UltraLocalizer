using Globe.Shared.DTOs;
using System.Collections.Generic;

namespace Globe.Shared.DTOs
{
    public class NotTranslatedConceptView
    {
        public int Id { get; set; }
        public ComponentNamespace ComponentNamespace { get; set; }
        public InternalNamespace InternalNamespace { get; set; }
        public string Name { get; set; }
        public IEnumerable<JobListContext> ContextViews { get; set; }
    }
}
