using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.BookingSystem.Queries
{
    public record GetStudentBookingsQuery(string StudentId) : IRequest<IEnumerable<BookingDto>>;
}
