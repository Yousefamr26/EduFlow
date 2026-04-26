using FluentValidation;

namespace EduFlow.Infrastructure.Features.Ratings.Commands
{
    public class RateTeacherValidator : AbstractValidator<RateTeacherCommand>
    {
        public RateTeacherValidator()
        {
            RuleFor(x => x.TeacherId).NotEmpty().WithMessage("Teacher is required.");
            RuleFor(x => x.StudentId).NotEmpty().WithMessage("Student is required.");
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
            RuleFor(x => x.Comment)
                .MaximumLength(500).WithMessage("Comment must not exceed 500 characters.");
        }
    }
}