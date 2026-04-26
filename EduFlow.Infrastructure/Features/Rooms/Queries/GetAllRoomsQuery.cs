using MediatR;

namespace EduFlow.Infrastructure.Features.Rooms.Queries
{
    public record RoomDto(int Id, string Name, int Capacity, string? Location, bool IsAvailable);

    public record GetAllRoomsQuery() : IRequest<IEnumerable<RoomDto>>;
}