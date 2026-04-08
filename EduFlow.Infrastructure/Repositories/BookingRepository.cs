using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(EduDbContext context) : base(context) { }

    public async Task<bool> IsAlreadyBookedAsync(string studentId, int sessionId)
    {
        return await _dbSet.AnyAsync(b =>
            b.StudentId == studentId &&
            b.SessionId == sessionId);
    }

    public async Task<bool> HasTimeConflictAsync(string studentId, DateTime dateTime)
    {
        return await _dbSet
            .Include(b => b.Session)
            .AnyAsync(b =>
                b.StudentId == studentId &&
                b.Session.DateTime == dateTime);
    }

    public async Task<IEnumerable<Booking>> GetStudentBookingsAsync(string studentId)
    {
        return await _dbSet
            .Where(b => b.StudentId == studentId)
            .Include(b => b.Student) 
            .Include(b => b.Session) 
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsBySessionIdAsync(int sessionId)
    {
        return await _dbSet
            .Where(b => b.SessionId == sessionId)
            .Include(b => b.Student)
            .Include(b => b.Session)
            .ToListAsync();
    }
}