using FluentValidation;
using MyLabLocalizer.LocalizationService.DTOs;

namespace MyLabLocalizer.LocalizationService.Validations
{
    public class JobListSearchDTOValidator : AbstractValidator<JobItemSearchDTO>
    {
        public JobListSearchDTOValidator()
        {
            RuleFor(model => model.ISOCoding)
                .NotEmpty();
        }
    }
}
