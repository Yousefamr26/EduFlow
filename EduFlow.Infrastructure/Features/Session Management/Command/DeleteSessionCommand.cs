using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.Session_Management
{
    public class DeleteSessionCommand : IRequest<Unit>
    {
        public int SessionId { get; }
        public string TeacherId { get; } 

        public DeleteSessionCommand(int sessionId, string teacherId)
        {
            SessionId = sessionId;
            TeacherId = teacherId;
        }

    }
}
