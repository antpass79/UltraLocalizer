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
            RuleFor(model => model.Concept.EditableContexts
                .All(localizeString => !string.IsNullOrEmpty(localizeString.StringEditableValue)));              
            RuleFor(model => model.Language)
                .NotEmpty();
        }
    }
}
