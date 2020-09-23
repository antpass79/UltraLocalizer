using System.Collections.Generic;

namespace Globe.TranslationServer.DTOs
{
    public class InternalNamespaceGroupDTO
    {
        public ComponentNamespaceDTO ComponentNamespace { get; set; }

        public IEnumerable<InternalNamespaceDTO> InternalNamespaces { get; set; }
    }
}
