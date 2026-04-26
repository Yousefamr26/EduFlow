using MediatR;

namespace EduFlow.Infrastructure.Features.TeacherSubjects.Queries
{
    public record TeacherSubjectDto(int SubjectId, string SubjectName, string? Description);

    public record GetSubjectsByTeacherQuery(string TeacherId) : IRequest<IEnumerable<TeacherSubjectDto>>;
}