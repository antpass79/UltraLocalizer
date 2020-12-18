using Globe.Shared.DTOs;

namespace Globe.Client.Localizer.Models
{
    class SavableConcept
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Language Language { get; set; }
        public BindableComponentNamespace ComponentNamespace { get; set; }
        public BindableInternalNamespace InternalNamespace { get; set; }
    }
}
