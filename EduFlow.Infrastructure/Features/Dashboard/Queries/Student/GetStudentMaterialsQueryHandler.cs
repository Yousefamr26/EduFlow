using AutoMapper;
using EduFlow.Application.Interfaces.UnitOfWork;
using MediatR;

public class GetStudentMaterialsQueryHandler : IRequestHandler<GetStudentMaterialsQuery, IEnumerable<MaterialDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetStudentMaterialsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        => (_unitOfWork, _mapper) = (unitOfWork, mapper);

    public async Task<IEnumerable<MaterialDto>> Handle(GetStudentMaterialsQuery request, CancellationToken cancellationToken)
    {
        var alreadyBooked = await _unitOfWork.Bookings.IsAlreadyBookedAsync(request.StudentId, request.SessionId);
        if (!alreadyBooked) throw new Exception("You did not book this session");

        var materials = await _unitOfWork.Materials.GetBySessionIdAsync(request.SessionId);
        return _mapper.Map<IEnumerable<MaterialDto>>(materials);
    }
}