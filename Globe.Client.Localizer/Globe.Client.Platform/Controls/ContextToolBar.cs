using Prism.Regions;
using System.Windows.Controls;

namespace Globe.Client.Platform.Controls
{
    public abstract class ContextToolBar : UserControl, INavigationAware
    {
        protected ContextToolBar()
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            OnDataContextLinked(navigationContext.NavigationService.Region.Context);
        }

        protected virtual void OnDataContextLinked(object dataContext)
        {
            this.DataContext = dataContext;
        }
    }
}
