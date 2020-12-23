using Globe.Shared.DTOs;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Globe.Client.Localizer.Models
{
    public class BindableComponentNamespaceGroup : ComponentNamespaceGroup<BindableComponentNamespace, BindableInternalNamespace>, INotifyPropertyChanged
    {
        bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public int Count
        {
            get { return InternalNamespaces != null ? InternalNamespaces.Count() : 0; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}