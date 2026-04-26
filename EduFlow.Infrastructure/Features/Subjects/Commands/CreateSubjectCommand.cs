using MediatR;

namespace EduFlow.Infrastructure.Features.Subjects.Commands
{
    public record CreateSubjectCommand(
        string Name,
        string? Description
    ) : IRequest<string>;
}