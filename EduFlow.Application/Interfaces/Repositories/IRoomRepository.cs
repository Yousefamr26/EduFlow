using EduFlow.Domain.Entities;

namespace EduFlow.Application.Interfaces.Repositories
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<IEnumerable<Room>> GetAvailableRoomsAsync();
        Task<bool> HasConflictAsync(int roomId, DateTime dateTime);
    }
}