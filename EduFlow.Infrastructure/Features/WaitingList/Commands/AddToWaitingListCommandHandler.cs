using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Domain.Entities;
using MediatR;

namespace EduFlow.Infrastructure.Features.WaitingList.Commands
{
    public class AddToWaitingListCommandHandler : IRequestHandler<AddToWaitingListCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddToWaitingListCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(AddToWaitingListCommand request, CancellationToken cancellationToken)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(request.SessionId);
            if (session == null || session.IsCanceled)
                throw new Exception("Session not available");

            // Check if already in waiting list
            if (await _unitOfWork.WaitingList.IsInWaitingListAsync(request.StudentId, request.SessionId))
                throw new Exception("Already in waiting list for this session");

            // Check if already booked
            if (await _unitOfWork.Bookings.IsAlreadyBookedAsync(request.StudentId, request.SessionId))
                throw new Exception("Already booked for this session");

            var existingEntries = await _unitOfWork.WaitingList.GetWaitingListBySessionIdAsync(request.SessionId);
            var nextPosition = existingEntries.Count() + 1;

            var waitingEntry = new WaitingListEntry
            {
                StudentId = request.StudentId,
                SessionId = request.SessionId,
                RequestTime = DateTime.UtcNow,
                QueuePosition = nextPosition
            };

            await _unitOfWork.WaitingList.AddAsync(waitingEntry);
            await _unitOfWork.SaveChangesAsync();

            return waitingEntry.Id;
        }
    }
}
