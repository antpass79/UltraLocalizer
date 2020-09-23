using Prism.Mvvm;

namespace Globe.Client.Localizer.Models
{
    class InternalNamespace : BindableBase
    {
        public string Description { get; set; }
        
        bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SetProperty<bool>(ref _isSelected, value);
            }
        }
    }
}
