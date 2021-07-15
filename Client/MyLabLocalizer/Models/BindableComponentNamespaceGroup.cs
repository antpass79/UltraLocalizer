using MyLabLocalizer.Shared.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MyLabLocalizer.Models
{
    public class BindableComponentNamespaceGroup : ComponentNamespaceGroup<BindableComponentNamespace, BindableInternalNamespace>, INotifyPropertyChanged
    {
        bool _semaphore = false;

        bool? _isSelected = false;
        public bool? IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public int Count
        {
            get { return InternalNamespaces != null ? InternalNamespaces.Count() : 0; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override IEnumerable<BindableInternalNamespace> CreateInternalNamespacesEnumerable(IEnumerable<BindableInternalNamespace> internalNamespaces)
        {
            if (this.InternalNamespaces != null)
            {
                (this.InternalNamespaces as ObservableCollection<BindableInternalNamespace>).ToList().ForEach(item => item.PropertyChanged -= Item_PropertyChanged);
                (this.InternalNamespaces as ObservableCollection<BindableInternalNamespace>).CollectionChanged -= BindableInternalNamespaces_CollectionChanged;
            }

            var bindableInternalNamespaces = new ObservableCollection<BindableInternalNamespace>(internalNamespaces);

            bindableInternalNamespaces.CollectionChanged += BindableInternalNamespaces_CollectionChanged;
            bindableInternalNamespaces.ToList().ForEach(item => item.PropertyChanged += Item_PropertyChanged);

            return bindableInternalNamespaces;
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(IsSelected) && !_semaphore)
            {             
                _semaphore = true;
                var any = this.InternalNamespaces.Cast<BindableInternalNamespace>().Where(item => item.IsSelected);
                var all = this.InternalNamespaces;
                this.IsSelected = any.Count() == all.Count() ? true : (any.Count() == 0 ? false : null);
                _semaphore = false;
            }
        }

        private void BindableInternalNamespaces_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var result = e.Action switch
            {
                NotifyCollectionChangedAction.Add => AddItems(e.NewItems),
                NotifyCollectionChangedAction.Remove => RemoveItems(e.OldItems),
                NotifyCollectionChangedAction.Reset => RemoveItems(e.OldItems),
                _ => throw new NotImplementedException()
            };
        }

#nullable enable

        private bool AddItems(IList? items)
        {
            if (items == null)
                return true;

            items.Cast<INotifyPropertyChanged>().ToList().ForEach(item => item.PropertyChanged += Item_PropertyChanged); return true;
        }

        private bool RemoveItems(IList? items)
        {
            if (items == null)
                return true;

            items.Cast<INotifyPropertyChanged>().ToList().ForEach(item => item.PropertyChanged -= Item_PropertyChanged); return true;
        }

#nullable disable

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(IsSelected) && IsSelected.HasValue && !_semaphore)
            {
                _semaphore = true;
                this.InternalNamespaces.Cast<BindableInternalNamespace>().ToList().ForEach(item => item.IsSelected = this.IsSelected.Value);
                _semaphore = false;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}