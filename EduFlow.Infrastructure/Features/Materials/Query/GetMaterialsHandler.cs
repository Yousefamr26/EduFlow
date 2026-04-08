using AutoMapper;
using EduFlow.Application.Interfaces.UnitOfWork;
using MediatR;

public class GetMaterialsHandler : IRequestHandler<GetMaterialsBySessionQuery, IEnumerable<MaterialDto>>
{
    private readonly IUnitOfWork _unit;
    private readonly IMapper _mapper;

    public GetMaterialsHandler(IUnitOfWork unit, IMapper mapper)
    {
        _unit = unit;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MaterialDto>> Handle(GetMaterialsBySessionQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.StudentId))
            throw new Exception("StudentId not provided");

        var isBooked = await _unit.Bookings.IsAlreadyBookedAsync(request.StudentId, request.SessionId);
        if (!isBooked)
            throw new Exception("Unauthorized");

        var materials = await _unit.Materials.GetBySessionIdAsync(request.SessionId);

        return _mapper.Map<IEnumerable<MaterialDto>>(materials);
    }
}