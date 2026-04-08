using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Infrastructure.Features.Session_Management;
using MediatR;

public class DeleteSessionHandler : IRequestHandler<DeleteSessionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSessionHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions.GetByIdAsync(request.SessionId);
        if (session == null)
            throw new Exception("Session not found");

        if (session.TeacherId != request.TeacherId)
            throw new Exception("You are not authorized to delete this session");

        session.IsCanceled = true;
        _unitOfWork.Sessions.Update(session);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}