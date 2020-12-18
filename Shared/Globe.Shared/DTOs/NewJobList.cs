using System.Collections.Generic;

namespace Globe.Shared.DTOs
{
    public class NewJobList
    {
        public string Name { get; set; }
        public ApplicationUser User { get; set; }
        public Language Language { get; set; }
        public IEnumerable<NotTranslatedConceptView> Concepts { get; set; }
    }
}
