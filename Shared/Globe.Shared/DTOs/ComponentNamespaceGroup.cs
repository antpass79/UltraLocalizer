using System.Collections.Generic;

namespace Globe.Shared.DTOs
{
    public class ComponentNamespaceGroup<TComponent, TInternal>
        where TComponent : ComponentNamespace
        where TInternal : InternalNamespace
    {
        public TComponent ComponentNamespace { get; set; }

        public IEnumerable<TInternal> InternalNamespaces { get; set; }
    }
    public class ComponentNamespaceGroup : ComponentNamespaceGroup<ComponentNamespace, InternalNamespace>
    {
    }
}