using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.Infrastructure.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        private readonly EduDbContext _context;

        public RoomRepository(EduDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync()
            => await _context.Rooms
                .Where(r => r.IsAvailable && !r.IsDeleted)
                .ToListAsync();

        public async Task<bool> HasConflictAsync(int roomId, DateTime dateTime)
            => await _context.Sessions
                .AnyAsync(s => s.RoomId == roomId && s.DateTime == dateTime && !s.IsCanceled);
    }
}