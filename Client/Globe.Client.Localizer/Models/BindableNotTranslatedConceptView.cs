﻿using Globe.Shared.DTOs;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Globe.Client.Localizer.Models
{
    class BindableNotTranslatedConceptView : NotTranslatedConceptView, INotifyPropertyChanged
    {
        bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
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