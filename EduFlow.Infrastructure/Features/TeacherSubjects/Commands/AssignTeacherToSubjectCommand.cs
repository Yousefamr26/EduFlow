using MediatR;

namespace EduFlow.Infrastructure.Features.TeacherSubjects.Commands
{
    public record AssignTeacherToSubjectCommand(
        string TeacherId,
        int SubjectId
    ) : IRequest<string>;
}