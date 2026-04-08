using AutoMapper;
using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Infrastructure.Query;
using MediatR;


public class GetStudentUpcomingSessionsQueryHandler : IRequestHandler<GetStudentUpcomingSessionsQuery, IEnumerable<SessionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetStudentUpcomingSessionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        => (_unitOfWork, _mapper) = (unitOfWork, mapper);

    public async Task<IEnumerable<SessionDto>> Handle(GetStudentUpcomingSessionsQuery request, CancellationToken cancellationToken)
    {
        var bookings = await _unitOfWork.Bookings.GetStudentBookingsAsync(request.StudentId);
        var upcomingSessions = bookings
            .Where(b => !b.Session.IsCanceled && b.Session.DateTime > DateTime.UtcNow)
            .Select(b => b.Session)
            .OrderBy(s => s.DateTime);

        return _mapper.Map<IEnumerable<SessionDto>>(upcomingSessions);
    }
}