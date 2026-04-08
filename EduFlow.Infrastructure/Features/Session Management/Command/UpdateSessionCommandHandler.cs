using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Application.Interfaces.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.Session_Management
{
    public class UpdateSessionCommandHandler : IRequestHandler<UpdateSessionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSessionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateSessionCommand request, CancellationToken cancellationToken)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(request.SessionId);
            if (session == null)
                throw new Exception("Session not found");

            if (await _unitOfWork.Sessions.HasConflictAsync(request.TeacherId, request.DateTime))
            {
                if (session.DateTime != request.DateTime) 
                    throw new Exception("Teacher has another session at this time");
            }

            session.Title = request.Title;
            session.Description = request.Description;
            session.DateTime = request.DateTime;
            session.Capacity = request.Capacity;

            if (session.Capacity < session.BookedCount)
                throw new Exception("Capacity cannot be less than booked seats");

            _unitOfWork.Sessions.Update(session);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
