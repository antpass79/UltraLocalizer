using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace Globe.Client.Localizer.Models
{
    class InternalNamespaceGroup : BindableBase
    {
        public ComponentNamespace ComponentNamespace { get; set; }

        public IEnumerable<InternalNamespace> InternalNamespaces { get; set; }

        public int Count
        {
            get { return InternalNamespaces != null ? InternalNamespaces.Count() : 0; }
        }
    }
}