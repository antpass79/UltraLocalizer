using MyLabLocalizer.Shared.DTOs;

namespace MyLabLocalizer.Models
{
    class SavableConceptModel
    {
        public SavableConceptModel(Language language, EditableConcept concept)
        {
            Language = language;
            Concept = concept;
        }

        public Language Language { get; }
        public EditableConcept Concept { get; }
    }
}
