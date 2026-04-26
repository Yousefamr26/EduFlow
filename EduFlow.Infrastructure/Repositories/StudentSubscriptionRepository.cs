using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.Infrastructure.Repositories
{
    public class StudentSubscriptionRepository : GenericRepository<StudentSubscription>, IStudentSubscriptionRepository
    {
        private readonly EduDbContext _context;

        public StudentSubscriptionRepository(EduDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<StudentSubscription?> GetActiveSubscriptionAsync(string studentId)
            => await _context.StudentSubscriptions
                .Include(s => s.Package)
                .Where(s => s.StudentId == studentId && !s.IsDeleted
                    && s.StartDate <= DateTime.UtcNow
                    && s.EndDate >= DateTime.UtcNow)
                .FirstOrDefaultAsync();

        public async Task<bool> HasActiveSubscriptionAsync(string studentId)
            => await _context.StudentSubscriptions
                .AnyAsync(s => s.StudentId == studentId && !s.IsDeleted
                    && s.StartDate <= DateTime.UtcNow
                    && s.EndDate >= DateTime.UtcNow);
    }
}