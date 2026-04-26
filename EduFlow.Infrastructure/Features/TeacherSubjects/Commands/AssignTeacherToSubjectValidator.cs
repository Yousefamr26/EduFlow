using FluentValidation;

namespace EduFlow.Infrastructure.Features.TeacherSubjects.Commands
{
    public class AssignTeacherToSubjectValidator : AbstractValidator<AssignTeacherToSubjectCommand>
    {
        public AssignTeacherToSubjectValidator()
        {
            RuleFor(x => x.TeacherId).NotEmpty().WithMessage("Teacher is required.");
            RuleFor(x => x.SubjectId).GreaterThan(0).WithMessage("Subject is required.");
        }
    }
}