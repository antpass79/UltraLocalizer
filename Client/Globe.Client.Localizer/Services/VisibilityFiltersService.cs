namespace Globe.Client.Localizer.Services
{
    public class VisibilityFiltersService : IVisibilityFiltersService
    {
        bool IVisibilityFiltersService.Visible { get; set; } = true;
    }
}
