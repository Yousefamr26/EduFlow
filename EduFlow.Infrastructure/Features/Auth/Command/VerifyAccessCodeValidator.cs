using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.Auth.Command
{
    public class VerifyAccessCodeValidator : AbstractValidator<VerifyAccessCodeCommand>
    {
        public VerifyAccessCodeValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
