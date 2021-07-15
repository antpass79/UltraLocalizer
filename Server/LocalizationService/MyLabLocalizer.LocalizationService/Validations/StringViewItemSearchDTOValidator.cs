using FluentValidation;
using MyLabLocalizer.Shared.DTOs;

namespace MyLabLocalizer.LocalizationService.Validations
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
