using EduFlow.Application.Interfaces.UnitOfWork;
using MediatR;

namespace EduFlow.Infrastructure.Features.WaitingList.Queries
{
    public class GetStudentWaitingListPositionsQueryHandler : IRequestHandler<GetStudentWaitingListPositionsQuery, IEnumerable<StudentWaitingListPositionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStudentWaitingListPositionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<StudentWaitingListPositionDto>> Handle(GetStudentWaitingListPositionsQuery request, CancellationToken cancellationToken)
        {
            var waitingEntries = await _unitOfWork.WaitingList.GetWaitingListBySessionIdAsync(0);

            // Get all waiting list entries for this student across all sessions
            var studentWaitingEntries = waitingEntries
                .Where(w => w.StudentId == request.StudentId)
                .ToList();

            var results = new List<StudentWaitingListPositionDto>();

            foreach (var entry in studentWaitingEntries)
            {
                var sessionWaitingList = await _unitOfWork.WaitingList.GetWaitingListBySessionIdAsync(entry.SessionId);
                var totalInQueue = sessionWaitingList.Count();

                var session = await _unitOfWork.Sessions.GetByIdAsync(entry.SessionId);
                if (session != null)
                {
                    results.Add(new StudentWaitingListPositionDto
                    {
                        SessionId = entry.SessionId,
                        SessionTitle = session.Title,
                        SessionDateTime = session.DateTime,
                        QueuePosition = entry.QueuePosition,
                        TotalInQueue = totalInQueue
                    });
                }
            }

            return results;
        }
    }
}
