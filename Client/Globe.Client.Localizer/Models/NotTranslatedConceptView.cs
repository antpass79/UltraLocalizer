using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Globe.Client.Localizer.Models
{
    class NotTranslatedConceptView : BindableBase
    {
        public int Id { get; set; }
        public ComponentNamespace ComponentNamespace { get; set; }
        public InternalNamespace InternalNamespace { get; set; }
        public string Name { get; set; }
        public IEnumerable<ContextView> ContextViews { get; set; }
        
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