using System.Collections.Generic;

namespace Globe.TranslationServer.DTOs
{
    public class ConceptViewDTO
    {
        public int Id { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Name { get; set; }
        public IEnumerable<ContextViewDTO> ContextViews { get; set; }

        public string DetailsLink { get; } = "ConceptDetails";
    }
}
