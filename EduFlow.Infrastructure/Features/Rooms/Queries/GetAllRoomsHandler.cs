using EduFlow.Application.Interfaces.Repositories;
using MediatR;

namespace EduFlow.Infrastructure.Features.Rooms.Queries
{
    public class GetAllRoomsHandler : IRequestHandler<GetAllRoomsQuery, IEnumerable<RoomDto>>
    {
        private readonly IRoomRepository _roomRepository;

        public GetAllRoomsHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<RoomDto>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
        {
            var rooms = await _roomRepository.GetAvailableRoomsAsync();

            return rooms.Select(r => new RoomDto(r.Id, r.Name, r.Capacity, r.Location, r.IsAvailable));
        }
    }
}