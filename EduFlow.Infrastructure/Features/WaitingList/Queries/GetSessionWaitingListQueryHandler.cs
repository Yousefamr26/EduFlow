using EduFlow.Application.Interfaces.UnitOfWork;
using MediatR;

namespace EduFlow.Infrastructure.Features.WaitingList.Queries
{
    public class GetSessionWaitingListQueryHandler : IRequestHandler<GetSessionWaitingListQuery, SessionWaitingListDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSessionWaitingListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SessionWaitingListDto> Handle(GetSessionWaitingListQuery request, CancellationToken cancellationToken)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(request.SessionId);
            if (session == null)
                throw new Exception("Session not found");

            var waitingEntries = await _unitOfWork.WaitingList.GetWaitingListBySessionIdAsync(request.SessionId);

            var result = new SessionWaitingListDto
            {
                SessionId = session.Id,
                SessionTitle = session.Title,
                SessionDateTime = session.DateTime,
                TotalWaiting = waitingEntries.Count(),
                Entries = waitingEntries
                    .Select(w => new WaitingListEntryDto
                    {
                        Id = w.Id,
                        StudentId = w.StudentId,
                        StudentName = w.Student?.Name ?? "Unknown",
                        SessionId = w.SessionId,
                        SessionTitle = session.Title,
                        RequestTime = w.RequestTime,
                        QueuePosition = w.QueuePosition
                    })
                    .ToList()
            };

            return result;
        }
    }
}
