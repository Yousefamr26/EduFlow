using MediatR;

namespace EduFlow.Infrastructure.Features.Subscriptions.Commands
{
    public record SubscribeCommand(
        string StudentId,
        int PackageId
    ) : IRequest<string>;
}