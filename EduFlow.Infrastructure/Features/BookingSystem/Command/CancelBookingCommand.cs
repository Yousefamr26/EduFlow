using MediatR;

namespace EduFlow.Infrastructure.Features.BookingSystem.Command
{
    public record CancelBookingCommand(int BookingId, string StudentId) : IRequest<Unit>;
}