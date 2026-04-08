using AutoMapper;
using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Application.Interfaces.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.BookingSystem.Queries
{
    public class GetStudentBookingsQueryHandler : IRequestHandler<GetStudentBookingsQuery, IEnumerable<BookingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetStudentBookingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            => (_unitOfWork, _mapper) = (unitOfWork, mapper);

        public async Task<IEnumerable<BookingDto>> Handle(GetStudentBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _unitOfWork.Bookings.GetStudentBookingsAsync(request.StudentId);
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }
    }
}
