using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Globe.Client.Localizer.Models
{
    public class InternalNamespaceGroup
    {
        public ComponentNamespace ComponentNamespace { get; set; }

        public IEnumerable<InternalNamespace> InternalNamespaces { get; set; }
    }
}