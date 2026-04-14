using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.Infrastructure.Repositories
{
    public class WaitingListRepository : GenericRepository<WaitingListEntry>, IWaitingListRepository
    {
        public WaitingListRepository(EduDbContext context) : base(context) { }

        public async Task<WaitingListEntry> GetWaitingListEntryAsync(string studentId, int sessionId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(w => w.StudentId == studentId && w.SessionId == sessionId);
        }

        public async Task<bool> IsInWaitingListAsync(string studentId, int sessionId)
        {
            return await _dbSet.AnyAsync(w => w.StudentId == studentId && w.SessionId == sessionId);
        }

        public async Task<IEnumerable<WaitingListEntry>> GetWaitingListBySessionIdAsync(int sessionId)
        {
            return await _dbSet
                .Where(w => w.SessionId == sessionId)
                .OrderBy(w => w.QueuePosition)
                .Include(w => w.Student)
                .ToListAsync();
        }

        public async Task<WaitingListEntry> GetFirstInQueueAsync(int sessionId)
        {
            return await _dbSet
                .Where(w => w.SessionId == sessionId)
                .OrderBy(w => w.QueuePosition)
                .FirstOrDefaultAsync();
        }

        public async Task RemoveFromWaitingListAsync(string studentId, int sessionId)
        {
            var entry = await GetWaitingListEntryAsync(studentId, sessionId);
            if (entry != null)
            {
                _dbSet.Remove(entry);
            }
        }

        public async Task UpdateQueuePositionsAsync(int sessionId)
        {
            var waitingEntries = await _dbSet
                .Where(w => w.SessionId == sessionId)
                .OrderBy(w => w.RequestTime)
                .ToListAsync();

            for (int i = 0; i < waitingEntries.Count; i++)
            {
                waitingEntries[i].QueuePosition = i + 1;
                _dbSet.Update(waitingEntries[i]);
            }
        }
    }
}
