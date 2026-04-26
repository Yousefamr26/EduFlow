using MediatR;

namespace EduFlow.Infrastructure.Features.Rooms.Commands
{
    public record CreateRoomCommand(
        string Name,
        int Capacity,
        string? Location
    ) : IRequest<string>;
}