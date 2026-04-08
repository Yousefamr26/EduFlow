using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace EduFlow.Infrastructure.Features.Session_Management
{
    

    public class CreateSessionValidator : AbstractValidator<CreateSessionCommandWithTeacherId>
    {
        public CreateSessionValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.DateTime)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Session date must be in the future");
            RuleFor(x => x.Capacity)
                .GreaterThan(0)
                .WithMessage("Capacity must be greater than 0");
          
        }
    }
}
