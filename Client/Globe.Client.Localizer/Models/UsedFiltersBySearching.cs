using Globe.Shared.DTOs;

namespace Globe.Client.Localizer.Models
{
    class UsedFiltersBySearching
    {
        public Language Language { get; set; }
        public JobItem JobItem { get; set; }
        public BindableComponentNamespace ComponentNamespace { get; set; }
        public BindableInternalNamespace InternalNamespace { get; set; }
    }
}
