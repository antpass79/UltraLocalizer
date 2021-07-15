using MyLabLocalizer.Shared.DTOs;

namespace MyLabLocalizer.LocalizationService.DTOs
{
    public class SavableConceptModelDTO
    {
        public Language Language { get; set; }
        public EditableConceptDTO Concept { get; set; }
    }
}
