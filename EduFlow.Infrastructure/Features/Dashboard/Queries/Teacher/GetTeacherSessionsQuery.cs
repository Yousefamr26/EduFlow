using EduFlow.Infrastructure.Query;
using MediatR;

public record GetTeacherSessionsQuery(string TeacherId) : IRequest<IEnumerable<SessionDto>>;
