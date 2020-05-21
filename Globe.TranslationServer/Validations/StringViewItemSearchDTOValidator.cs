using FluentValidation;
using Globe.TranslationServer.DTOs;

namespace Globe.TranslationServer.Validations
{
    public class StringViewItemSearchDTOValidator : AbstractValidator<ConceptViewSearchDTO>
    {
        public StringViewItemSearchDTOValidator()
        {
            RuleFor(model => model.ComponentNamespace)
                .NotEmpty();
            RuleFor(model => model.InternalNamespace)
                .NotEmpty();
            RuleFor(model => model.ISOCoding)
                .NotEmpty();
            RuleFor(model => model.JobListId)
                .NotEmpty();
        }
    }
}
