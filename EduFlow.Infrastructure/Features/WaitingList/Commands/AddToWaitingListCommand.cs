using MediatR;

namespace EduFlow.Infrastructure.Features.WaitingList.Commands
{
    public record AddToWaitingListCommand(string StudentId, int SessionId) : IRequest<int>;
}
