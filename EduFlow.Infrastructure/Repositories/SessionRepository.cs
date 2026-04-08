using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Repositories
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        public SessionRepository(EduDbContext context) : base(context) { }

        public async Task<IEnumerable<Session>> GetAvailableSessionsAsync()
        {
            return await _dbSet
                .Where(s => !s.IsCanceled &&
                            s.DateTime > DateTime.UtcNow &&
                            s.BookedCount < s.Capacity)
                .ToListAsync();
        }

        public async Task<bool> HasConflictAsync(string teacherId, DateTime dateTime)
        {
            return await _dbSet.AnyAsync(s =>
                s.TeacherId == teacherId &&
                s.DateTime == dateTime &&
                !s.IsCanceled);
        }
    }
}
