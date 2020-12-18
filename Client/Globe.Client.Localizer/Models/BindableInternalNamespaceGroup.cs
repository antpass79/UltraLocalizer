using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace Globe.Client.Localizer.Models
{
    public class BindableInternalNamespaceGroup : BindableBase
    {
        bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SetProperty(ref _isSelected, value);
            }
        }

        BindableComponentNamespace _componentNamespace;
        public BindableComponentNamespace ComponentNamespace
        {
            get => _componentNamespace;
            set
            {
                SetProperty(ref _componentNamespace, value);
            }
        }

        IEnumerable<BindableInternalNamespace> _internalNamespaces;
        public IEnumerable<BindableInternalNamespace> InternalNamespaces
        {
            get => _internalNamespaces;
            set
            {
                SetProperty(ref _internalNamespaces, value);
            }
        }

        public int Count
        {
            get { return InternalNamespaces != null ? InternalNamespaces.Count() : 0; }
        }
    }
}