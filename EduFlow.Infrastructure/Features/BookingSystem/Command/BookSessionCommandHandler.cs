using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.BookingSystem.Command
{
    public class BookSessionCommandHandler : IRequestHandler<BookSessionCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookSessionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(BookSessionCommand request, CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Auths.GetUserByIdAsync(request.StudentId);

            if (!student.IsAccessCodeVerified)
                throw new Exception("Access code verification required");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var session = await _unitOfWork.Sessions.GetByIdAsync(request.SessionId);

                if (session == null || session.IsCanceled)
                    throw new Exception("Session not available");

                if (session.DateTime <= DateTime.UtcNow)
                    throw new Exception("Session already started");

                if (await _unitOfWork.Bookings.IsAlreadyBookedAsync(request.StudentId, request.SessionId))
                    throw new Exception("Already booked");

                if (await _unitOfWork.Bookings.HasTimeConflictAsync(request.StudentId, session.DateTime))
                    throw new Exception("Time conflict");

                // If session is full, add to waiting list instead
                if (session.BookedCount >= session.Capacity)
                {
                    await _unitOfWork.RollbackAsync();

                    // Check if already in waiting list
                    if (await _unitOfWork.WaitingList.IsInWaitingListAsync(request.StudentId, request.SessionId))
                        throw new Exception("Already in waiting list for this session");

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

                    // Return negative ID to indicate it's a waiting list entry, not a booking
                    return -waitingEntry.Id;
                }

                var booking = new Booking
                {
                    StudentId = request.StudentId,
                    SessionId = request.SessionId,
                    BookingTime = DateTime.UtcNow
                };

                await _unitOfWork.Bookings.AddAsync(booking);

                session.BookedCount++;
                _unitOfWork.Sessions.Update(session);

                await _unitOfWork.CommitAsync();

                return booking.Id;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
