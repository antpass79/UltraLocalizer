using MyLabLocalizer.Shared.DTOs;

namespace MyLabLocalizer.Models
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
