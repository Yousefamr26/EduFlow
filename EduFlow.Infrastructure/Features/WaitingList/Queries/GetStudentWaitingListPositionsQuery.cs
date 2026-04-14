using MediatR;

namespace EduFlow.Infrastructure.Features.WaitingList.Queries
{
    public record GetStudentWaitingListPositionsQuery(string StudentId) : IRequest<IEnumerable<StudentWaitingListPositionDto>>;
}
