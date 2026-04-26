using FluentValidation;

namespace EduFlow.Infrastructure.Features.Rooms.Commands
{
    public class CreateRoomValidator : AbstractValidator<CreateRoomCommand>
    {
        public CreateRoomValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Room name is required.");
            RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("Capacity must be greater than 0.");
        }
    }
}