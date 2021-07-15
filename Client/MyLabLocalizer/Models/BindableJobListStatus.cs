using MyLabLocalizer.Shared.DTOs;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyLabLocalizer.Models
{
    public class BindableJobListStatus : JobListStatus, INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
