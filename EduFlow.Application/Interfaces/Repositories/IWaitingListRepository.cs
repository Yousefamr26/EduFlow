using EduFlow.Domain.Entities;

namespace EduFlow.Application.Interfaces.Repositories
{
    public interface IWaitingListRepository : IGenericRepository<WaitingListEntry>
    {
        Task<WaitingListEntry> GetWaitingListEntryAsync(string studentId, int sessionId);
        Task<bool> IsInWaitingListAsync(string studentId, int sessionId);
        Task<IEnumerable<WaitingListEntry>> GetWaitingListBySessionIdAsync(int sessionId);
        Task<WaitingListEntry> GetFirstInQueueAsync(int sessionId);
        Task RemoveFromWaitingListAsync(string studentId, int sessionId);
        Task UpdateQueuePositionsAsync(int sessionId);
    }
}
