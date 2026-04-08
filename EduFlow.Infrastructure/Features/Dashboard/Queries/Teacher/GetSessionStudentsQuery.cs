using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.Dashboard.Queries.Teacher
{
    public record GetSessionStudentsQuery(int SessionId) : IRequest<IEnumerable<StudentDto>>;

}
