using FluentValidation;

namespace EduFlow.Infrastructure.Features.Subjects.Commands
{
    public class CreateSubjectValidator : AbstractValidator<CreateSubjectCommand>
    {
        public CreateSubjectValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Subject name is required.")
                .MaximumLength(100);
        }
    }
}