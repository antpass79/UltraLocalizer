using System.Collections.Generic;

namespace Globe.TranslationServer.DTOs
{
    public class NotTranslatedConceptViewDTO
    {
        public int Id { get; set; }
        public ComponentNamespaceDTO ComponentNamespace { get; set; }
        public InternalNamespaceDTO InternalNamespace { get; set; }
        public string Name { get; set; }
        public IEnumerable<ContextViewDTO> ContextViews { get; set; }
    }
}
