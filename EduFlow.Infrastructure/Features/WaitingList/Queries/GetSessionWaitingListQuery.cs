using MediatR;
using EduFlow.Infrastructure.Features.WaitingList.Queries;

namespace EduFlow.Infrastructure.Features.WaitingList.Queries
{
    public record GetSessionWaitingListQuery(int SessionId) : IRequest<SessionWaitingListDto>;
}
