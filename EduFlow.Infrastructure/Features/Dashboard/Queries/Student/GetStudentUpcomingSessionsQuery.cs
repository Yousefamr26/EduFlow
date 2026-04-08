using EduFlow.Infrastructure.Query;
using MediatR;

public record GetStudentUpcomingSessionsQuery(string StudentId) : IRequest<IEnumerable<SessionDto>>;
