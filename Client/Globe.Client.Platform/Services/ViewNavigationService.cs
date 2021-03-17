using Globe.Client.Platofrm.Events;
using Prism.Events;
using Prism.Regions;
using System.Linq;

namespace Globe.Client.Platform.Services
{
    public class ViewNavigationService : IViewNavigationService
    {
        IEventAggregator _eventAggregator;
        IRegionManager _regionManager;
        public ViewNavigationService(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
        }

        public void NavigateTo(string toView)
        {
            _regionManager.RequestNavigate(RegionNames.MAIN_REGION, toView);            
            _regionManager.RequestNavigate(RegionNames.TOOLBAR_REGION, toView + ViewNames.TOOLBAR);

            _eventAggregator.GetEvent<ViewNavigationChangedEvent>().Publish(new ViewNavigation(toView));
        }

        public void NavigateTo<T>(string toView, T data) where T : class, new()
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("data", data);
            _regionManager.RequestNavigate(RegionNames.MAIN_REGION, toView, navigationParameters);
            _regionManager.RequestNavigate(RegionNames.TOOLBAR_REGION, toView + ViewNames.TOOLBAR, navigationParameters);

            _eventAggregator.GetEvent<ViewNavigationChangedEvent>().Publish(new ViewNavigation(toView));
        }
    }
}
