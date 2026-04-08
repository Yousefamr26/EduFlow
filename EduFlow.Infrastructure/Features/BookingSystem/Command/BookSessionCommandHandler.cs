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

                    if (session.BookedCount >= session.Capacity)
                        throw new Exception("Session full");

                    if (await _unitOfWork.Bookings.IsAlreadyBookedAsync(request.StudentId, request.SessionId))
                        throw new Exception("Already booked");

                    if (await _unitOfWork.Bookings.HasTimeConflictAsync(request.StudentId, session.DateTime))
                        throw new Exception("Time conflict");

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
