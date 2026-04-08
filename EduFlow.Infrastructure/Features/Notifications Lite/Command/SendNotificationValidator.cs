using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.Notifications_Lite
{
    public class SendNotificationValidator : AbstractValidator<SendNotificationCommand>
    {
        public SendNotificationValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Message).NotEmpty();
        }
    }
}
