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
    }
}
