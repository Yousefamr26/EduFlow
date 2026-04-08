using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.Notifications_Lite
{
    public record SendNotificationCommand(string UserId, string Message) : IRequest<int>;

}
