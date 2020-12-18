using Globe.Shared.DTOs;

namespace Globe.TranslationServer.DTOs
{
    public class SavableConceptModelDTO
    {
        public Language Language { get; set; }
        public EditableConceptDTO Concept { get; set; }
    }
}
