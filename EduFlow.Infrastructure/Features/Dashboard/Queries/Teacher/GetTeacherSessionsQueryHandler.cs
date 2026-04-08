using AutoMapper;
using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Infrastructure.Query;
using MediatR;


public class GetTeacherSessionsQueryHandler : IRequestHandler<GetTeacherSessionsQuery, IEnumerable<SessionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTeacherSessionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        => (_unitOfWork, _mapper) = (unitOfWork, mapper);

    public async Task<IEnumerable<SessionDto>> Handle(GetTeacherSessionsQuery request, CancellationToken cancellationToken)
    {
        var sessions = await _unitOfWork.Sessions.FindAsync(s => s.TeacherId == request.TeacherId && !s.IsCanceled);
        return _mapper.Map<IEnumerable<SessionDto>>(sessions);
    }
}