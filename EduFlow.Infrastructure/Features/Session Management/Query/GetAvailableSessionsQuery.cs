using EduFlow.Infrastructure.Query;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.Session_Management.Query
{

    public record GetAvailableSessionsQuery() : IRequest<IEnumerable<SessionDto>>;
}
