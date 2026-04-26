using MediatR;

namespace EduFlow.Infrastructure.Features.Ratings.Commands
{
    public record RateTeacherCommand(
        string StudentId,
        string TeacherId,
        int Rating,
        string? Comment
    ) : IRequest<string>;
}