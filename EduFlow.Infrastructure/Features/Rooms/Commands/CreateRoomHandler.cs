using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Domain.Entities;
using MediatR;

namespace EduFlow.Infrastructure.Features.Rooms.Commands
{
    public class CreateRoomHandler : IRequestHandler<CreateRoomCommand, string>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRoomHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = new Room
            {
                Name = request.Name,
                Capacity = request.Capacity,
                Location = request.Location,
                IsAvailable = true
            };

            await _roomRepository.AddAsync(room);
            await _unitOfWork.SaveChangesAsync();

            return $"Room '{request.Name}' created successfully.";
        }
    }
}