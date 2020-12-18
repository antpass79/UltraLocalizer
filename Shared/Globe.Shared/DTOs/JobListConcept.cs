using System.Collections.Generic;

namespace Globe.Shared.DTOs
{
    public class JobListConcept
    {
        public int Id { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Name { get; set; }
        public IList<JobListContext> ContextViews { get; set; }

        public string DetailsLink { get; } = "ConceptDetails";
    }
}
