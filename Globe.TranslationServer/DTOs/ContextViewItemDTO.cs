namespace Globe.TranslationServer.DTOs
{
    public class ContextViewItemDTO
    {
        public string Name { get; set; }
        public StringTypeDTO Type { get; set; }
        public string StringValue { get; set; }
        public int StringId { get; set; }
        public int Concept2ContextId { get; set; }
    }
}
