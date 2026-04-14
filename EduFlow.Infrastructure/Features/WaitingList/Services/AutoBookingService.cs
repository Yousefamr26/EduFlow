using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Domain.Entities;

namespace EduFlow.Infrastructure.Features.WaitingList.Services
{
    public interface IAutoBookingService
    {
        Task<bool> AutoBookNextFromWaitingListAsync(int sessionId);
    }

    public class AutoBookingService : IAutoBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AutoBookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AutoBookNextFromWaitingListAsync(int sessionId)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var session = await _unitOfWork.Sessions.GetByIdAsync(sessionId);
                if (session == null || session.IsCanceled)
                    return false;

                // Check if session still has capacity
                if (session.BookedCount >= session.Capacity)
                    return false;

                // Get first person in waiting list
                var nextInQueue = await _unitOfWork.WaitingList.GetFirstInQueueAsync(sessionId);
                if (nextInQueue == null)
                    return false;

                // Check for time conflicts
                if (await _unitOfWork.Bookings.HasTimeConflictAsync(nextInQueue.StudentId, session.DateTime))
                {
                    // Remove from waiting list if has conflict
                    await _unitOfWork.WaitingList.RemoveFromWaitingListAsync(nextInQueue.StudentId, sessionId);
                    await _unitOfWork.SaveChangesAsync();

                    // Try next person
                    await _unitOfWork.RollbackAsync();
                    return await AutoBookNextFromWaitingListAsync(sessionId);
                }

                // Create booking
                var booking = new Booking
                {
                    StudentId = nextInQueue.StudentId,
                    SessionId = sessionId,
                    BookingTime = DateTime.UtcNow
                };

                await _unitOfWork.Bookings.AddAsync(booking);
                session.BookedCount++;
                _unitOfWork.Sessions.Update(session);

                // Remove from waiting list
                await _unitOfWork.WaitingList.RemoveFromWaitingListAsync(nextInQueue.StudentId, sessionId);

                // Update queue positions for remaining entries
                await _unitOfWork.WaitingList.UpdateQueuePositionsAsync(sessionId);

                // Create notification
                var notification = new Notification
                {
                    UserId = nextInQueue.StudentId,
                    Message = $"Great news! A spot has opened up in '{session.Title}'. Your booking is confirmed!",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _unitOfWork.Notifications.AddAsync(notification);

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
