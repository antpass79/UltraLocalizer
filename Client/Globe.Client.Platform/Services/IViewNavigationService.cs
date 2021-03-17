namespace Globe.Client.Platform.Services
{
    public interface IViewNavigationService
    {
        void NavigateTo(string toView);
        void NavigateTo<T>(string toView, T data)
            where T: class, new();
    }
}
