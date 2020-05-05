using FluentValidation;
using Globe.TranslationServer.DTOs;

namespace Globe.TranslationServer.Validations
{
    public class JobListSearchDTOValidator : AbstractValidator<JobListSearchDTO>
    {
        public JobListSearchDTOValidator()
        {
            RuleFor(model => model.coding)
                .NotEmpty();
        }
    }
}
