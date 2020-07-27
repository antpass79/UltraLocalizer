using FluentValidation;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;

namespace Globe.AuthenticationServer
{
    public class RegistrationNewPasswordDTOValidator : AbstractValidator<RegistrationNewPasswordDTO>
    {
        public RegistrationNewPasswordDTOValidator()
        {
            RuleFor(model => model.Password)
                .NotEmpty()
                .MinimumLength(3);
            RuleFor(model => model.ConfirmPassword)
                .Equal(model => model.NewPassword);
            RuleFor(model => model.NewPassword)
                .NotEqual(model => model.Password);
        }
    }
}
