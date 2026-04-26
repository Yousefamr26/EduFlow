using MediatR;

namespace EduFlow.Infrastructure.Features.Subjects.Queries
{
    public record SubjectDto(int Id, string Name, string? Description);

    public record GetAllSubjectsQuery() : IRequest<IEnumerable<SubjectDto>>;
}