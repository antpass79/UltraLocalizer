namespace Globe.Client.Localizer.Models
{
    class SavableConcept
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Language Language { get; set; }
        public ComponentNamespace ComponentNamespace { get; set; }
        public InternalNamespace InternalNamespace { get; set; }
    }
}
