using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreateSessionHandler : IRequestHandler<CreateSessionCommandWithTeacherId, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateSessionHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateSessionCommandWithTeacherId request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Sessions.HasConflictAsync(request.TeacherId, request.DateTime))
            throw new Exception("Teacher has another session at this time");

        var session = new Session
        {
            Title = request.Title,
            Description = request.Description,
            DateTime = request.DateTime,
            Capacity = request.Capacity,
            TeacherId = request.TeacherId,
            SessionCodeHash = Guid.NewGuid().ToString("N"),
            RowVersion = new byte[8] 
        };

        await _unitOfWork.Sessions.AddAsync(session);
        await _unitOfWork.SaveChangesAsync();

        return session.Id;
    }
}