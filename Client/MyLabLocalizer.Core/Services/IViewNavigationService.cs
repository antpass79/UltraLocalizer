namespace MyLabLocalizer.Core.Services
{
    public interface IViewNavigationService
    {
        void NavigateTo(string toView);
        void NavigateTo(string toView, string fromView);
        void NavigateTo<T>(string toView, T data)
            where T: class, new();
        void NavigateTo<T>(string toView, string fromView, T data)
            where T : class, new();
    }
}
