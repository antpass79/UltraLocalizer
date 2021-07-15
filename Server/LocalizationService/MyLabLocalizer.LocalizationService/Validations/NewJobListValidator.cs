using FluentValidation;
using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.Services;
using System.Linq;

namespace MyLabLocalizer.LocalizationService.Validations
{
    public class NewJobListValidator : AbstractValidator<NewJobList>
    {
        private readonly IAsyncJobItemService _jobListService;
        public NewJobListValidator(IAsyncJobItemService jobListService)
        {
            _jobListService = jobListService;

            RuleFor(model => model.Name)
                .NotEmpty();
            RuleFor(model => model.Name)
                .MustAsync(async (name, cancellation) =>
                {
                    bool contains = (await _jobListService.GetAllAsync()).Select(item => item.Name).Contains(name);
                    return !contains;
                });
            RuleFor(model => model.Language)
                .NotEmpty();
            RuleFor(model => model.User)
                .NotEmpty();
            RuleFor(model => model.Concepts)
                .NotEmpty();
        }
    }
}
