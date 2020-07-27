using FluentValidation;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;

namespace Globe.AuthenticationServer
{
    public class RegistrationDTOValidator : AbstractValidator<RegistrationDTO>
    {
        public RegistrationDTOValidator()
        {
            RuleFor(model => model.UserName)
                .NotEmpty()
                .MaximumLength(25);
            RuleFor(model => model.Password)
                .NotEmpty()
                .MinimumLength(3);
            RuleFor(model => model.Email)
                .EmailAddress();
            RuleFor(model => model.ConfirmPassword)
                .Equal(model => model.Password);
        }
    }
}
