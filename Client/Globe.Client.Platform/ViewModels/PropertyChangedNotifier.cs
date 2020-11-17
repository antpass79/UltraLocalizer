using System.ComponentModel;

namespace Globe.Client.Platform.ViewModels
{
    public abstract class PropertyChangedNotifier : INotifyPropertyChanged
    {
        #region Constructors

        protected PropertyChangedNotifier()
        {
        }

        #endregion

        #region INotifyPropertyChanged Implementations

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected Functions

        virtual protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
