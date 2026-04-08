using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.Notifications_Lite.Queries
{

    public record GetUserNotificationsQuery(string UserId) : IRequest<IEnumerable<NotificationDto>>;
}
