using Prism.Mvvm;

namespace Globe.Client.Platform.Utilities
{
    public class SmartBusy : BindableBase
    {
        #region Data Members

        int _busyCount = 0;

        #endregion

        #region Properties

        bool _busy;
        public bool Busy
        {
            get => _busy;
            set
            {
                _busyCount = value ? _busyCount + 1 : _busyCount - 1;
                if (_busyCount <= 0)
                {
                    _busyCount = 0;
                    SetProperty(ref _busy, false);
                }
                else
                {
                    SetProperty(ref _busy, true);
                }
            }
        }

        #endregion
    }
}
