using Globe.Shared.DTOs;

namespace Globe.TranslationServer.DTOs
{
    public class EditableContextDTO
    {
        public EditableContextDTO()
        {
        }

        public string Name { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Concept { get; set; }
        public int Concept2ContextId { get; set; }
        public StringType StringType { get; set; }
        public string StringEditableValue { get; set; }
        public string StringDefaultValue { get; set; }
        public int StringId { get; set; }
        public int OldStringId { get; set; }
    }
}