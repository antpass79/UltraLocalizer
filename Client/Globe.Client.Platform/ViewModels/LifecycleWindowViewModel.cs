using Prism.Mvvm;
using Prism.Regions;
using System.Threading.Tasks;

namespace Globe.Client.Platform.ViewModels
{
    public class LifecycleWindowViewModel : BindableBase, INavigationAware
    {
        #region Properties

        bool _isInitialized;
        protected bool IsInitialized
        {
            get => _isInitialized;
            private set
            {
                SetProperty(ref _isInitialized, value);
            }
        }

        #endregion

        #region INavigationAware Interface

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (!IsInitialized)
                return;

            IsInitialized = false;

            OnUnload();
        }

        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            if (IsInitialized)
                return;

            IsInitialized = true;

            var fromView = navigationContext.Parameters.ContainsKey("fromView") ? navigationContext.Parameters["fromView"] as string : string.Empty;
            var data = navigationContext.Parameters.ContainsKey("data") ? navigationContext.Parameters["data"] : null;
            OnLoad(fromView, data);
        }

        #endregion

        #region Protected Functions

        virtual protected Task OnLoad(string fromView, object data)
        {
            return Task.CompletedTask;
        }

        virtual protected Task OnUnload()
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
