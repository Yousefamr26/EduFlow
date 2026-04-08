using AutoMapper;
using EduFlow.Application.Interfaces.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.Dashboard.Queries.Teacher
{
    public class GetSessionStudentsQueryHandler : IRequestHandler<GetSessionStudentsQuery, IEnumerable<StudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSessionStudentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            => (_unitOfWork, _mapper) = (unitOfWork, mapper);

        public async Task<IEnumerable<StudentDto>> Handle(GetSessionStudentsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _unitOfWork.Bookings.FindAsync(b => b.SessionId == request.SessionId);
            var students = bookings.Select(b => b.Student);
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }
    }
}
