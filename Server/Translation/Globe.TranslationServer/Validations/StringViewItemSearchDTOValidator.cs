using FluentValidation;
using Globe.Shared.DTOs;

namespace Globe.TranslationServer.Validations
{
    public class StringViewItemSearchDTOValidator : AbstractValidator<JobListConceptSearch>
    {
        public StringViewItemSearchDTOValidator()
        {
            RuleFor(model => model.ComponentNamespace)
                .NotEmpty();
            RuleFor(model => model.InternalNamespace)
                .NotEmpty();
            RuleFor(model => model.LanguageId)
                .NotEmpty();
        }
    }
}
