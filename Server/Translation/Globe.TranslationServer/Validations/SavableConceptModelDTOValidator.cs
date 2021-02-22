using FluentValidation;
using Globe.TranslationServer.DTOs;
using System.Linq;

namespace Globe.TranslationServer.Validations
{
    public class SavableConceptModelDTOValidator : AbstractValidator<SavableConceptModelDTO>
    {
        public SavableConceptModelDTOValidator()
        {
            RuleFor(model => model.Concept)
                .NotEmpty();
            RuleFor(model => model.Concept.EditableContexts)
                .NotEmpty();
            RuleForEach(model => model.Concept.EditableContexts)
                .Must(editableContext => !string.IsNullOrEmpty(editableContext.StringEditableValue));
            RuleFor(model => model.Language)
                .NotEmpty();
        }
    }
}
