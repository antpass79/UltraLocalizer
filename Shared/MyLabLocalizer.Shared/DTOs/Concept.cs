namespace MyLabLocalizer.Shared.DTOs
{
    public class ConceptDTO
    {
        public int Id { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Value { get; set; }
        public bool Ignore { get; set; }
        public string Comment { get; set; }
    }
}
