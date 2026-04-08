using AutoMapper;
using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Infrastructure.Query;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.Session_Management.Query
{
 

    public class GetAvailableSessionsQueryHandler : IRequestHandler<GetAvailableSessionsQuery, IEnumerable<SessionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAvailableSessionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SessionDto>> Handle(GetAvailableSessionsQuery request, CancellationToken cancellationToken)
        {
            var sessions = await _unitOfWork.Sessions.GetAvailableSessionsAsync();
            return _mapper.Map<IEnumerable<SessionDto>>(sessions);
        }
    }
}
