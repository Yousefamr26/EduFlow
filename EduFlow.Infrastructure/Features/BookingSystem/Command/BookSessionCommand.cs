using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.BookingSystem.Command
{
    public record BookSessionCommand(string StudentId, int SessionId) : IRequest<int>;

}
