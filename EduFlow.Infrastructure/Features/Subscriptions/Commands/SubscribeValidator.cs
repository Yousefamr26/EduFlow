using FluentValidation;

namespace EduFlow.Infrastructure.Features.Subscriptions.Commands
{
    public class SubscribeValidator : AbstractValidator<SubscribeCommand>
    {
        public SubscribeValidator()
        {
            RuleFor(x => x.StudentId).NotEmpty().WithMessage("Student is required.");
            RuleFor(x => x.PackageId).GreaterThan(0).WithMessage("Package is required.");
        }
    }
}