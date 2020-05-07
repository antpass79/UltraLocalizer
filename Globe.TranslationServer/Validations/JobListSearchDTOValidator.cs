using FluentValidation;
using Globe.TranslationServer.DTOs;

namespace Globe.TranslationServer.Validations
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
