using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Features.WaitingList.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.BookingSystem.Command
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoBookingService _autoBookingService;

        public CancelBookingCommandHandler(IUnitOfWork unitOfWork, IAutoBookingService autoBookingService)
        {
            _unitOfWork = unitOfWork;
            _autoBookingService = autoBookingService;
        }

        public async Task<Unit> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(request.BookingId);
            if (booking == null)
                throw new Exception("Booking not found");

            if (booking.StudentId != request.StudentId)
                throw new UnauthorizedAccessException("You are not allowed to cancel this booking");

            var session = await _unitOfWork.Sessions.GetByIdAsync(booking.SessionId);
            if (session == null)
                throw new Exception("Session not found");

            _unitOfWork.Bookings.Delete(booking);

            session.BookedCount = Math.Max(0, session.BookedCount - 1);
            _unitOfWork.Sessions.Update(session);

            var notification = new Notification
            {
                UserId = booking.StudentId,
                Message = $"Your booking for session '{session.Title}' has been canceled.",
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Notifications.AddAsync(notification);

            await _unitOfWork.SaveChangesAsync();

            // Auto-book next person from waiting list
            await _autoBookingService.AutoBookNextFromWaitingListAsync(booking.SessionId);

            return Unit.Value;
        }
    }
}