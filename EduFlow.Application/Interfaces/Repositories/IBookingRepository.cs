using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<bool> IsAlreadyBookedAsync(string studentId, int sessionId);
    Task<bool> HasTimeConflictAsync(string studentId, DateTime dateTime);

    Task<IEnumerable<Booking>> GetStudentBookingsAsync(string studentId);

    Task<IEnumerable<Booking>> GetBookingsBySessionIdAsync(int sessionId);
}