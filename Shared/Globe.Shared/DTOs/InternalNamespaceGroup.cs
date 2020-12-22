using Globe.Shared.DTOs;
using System.Collections.Generic;

namespace Globe.Shared.DTOs
{
    public class InternalNamespaceGroup<TComponent, TInternal>
        where TComponent : ComponentNamespace
        where TInternal : InternalNamespace
    {
        public TComponent ComponentNamespace { get; set; }

        public IEnumerable<TInternal> InternalNamespaces { get; set; }
    }
    public class InternalNamespaceGroup : InternalNamespaceGroup<ComponentNamespace, InternalNamespace>
    {
    }
}