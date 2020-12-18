using Prism.Mvvm;

namespace Globe.Client.Localizer.Models
{
    public class BindableInternalNamespace : BindableBase
    {
        public string Description { get; set; }

        bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SetProperty(ref _isSelected, value);
            }
        }
    }
}
