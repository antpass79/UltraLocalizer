using System.Collections.Generic;

namespace Globe.TranslationServer.DTOs
{
    public class StringViewItemDTO
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Concept { get; set; }
        public IEnumerable<ContextViewItemDTO> ContextViewItems { get; set; }
    }
}
