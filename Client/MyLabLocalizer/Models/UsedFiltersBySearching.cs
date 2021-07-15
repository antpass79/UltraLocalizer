using MyLabLocalizer.Shared.DTOs;

namespace MyLabLocalizer.Models
{
    class UsedFiltersBySearching
    {
        public Language Language { get; set; }
        public JobItem JobItem { get; set; }
        public BindableComponentNamespace ComponentNamespace { get; set; }
        public BindableInternalNamespace InternalNamespace { get; set; }
    }
}
