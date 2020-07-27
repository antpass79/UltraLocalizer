namespace Globe.TranslationServer.DTOs
{
    public class ContextViewDTO
    {
        public string Name { get; set; }
        public int Concept2ContextId { get; set; }

        public StringTypeDTO StringType { get; set; }
        public string StringValue { get; set; }
        public int StringId { get; set; }
        public int OldStringId { get; set; }
    }
}
