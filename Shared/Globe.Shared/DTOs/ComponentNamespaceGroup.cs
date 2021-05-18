using System;
using System.Collections.Generic;

namespace Globe.Shared.DTOs
{
    public class ComponentNamespaceGroup<TComponent, TInternal>
        where TComponent : ComponentNamespace
        where TInternal : InternalNamespace
    {
        public TComponent ComponentNamespace { get; set; }

        IEnumerable<TInternal> _internalNamespaces;
        public IEnumerable<TInternal> InternalNamespaces 
        {
            get { return _internalNamespaces; }
            set 
            {
                _internalNamespaces = CreateInternalNamespacesEnumerable(value); 
            } 
        }

        virtual protected IEnumerable<TInternal> CreateInternalNamespacesEnumerable(IEnumerable<TInternal> internalNamespaces)
        {
            return internalNamespaces;
        }
    }
    public class ComponentNamespaceGroup : ComponentNamespaceGroup<ComponentNamespace, InternalNamespace>
    {
    }
}