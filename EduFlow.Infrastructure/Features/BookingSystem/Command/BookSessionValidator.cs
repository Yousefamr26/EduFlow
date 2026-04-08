using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.BookingSystem.Command
{
    public class BookSessionValidator : AbstractValidator<BookSessionCommand>
    {
        public BookSessionValidator()
        {
            RuleFor(x => x.StudentId).NotEmpty();
            RuleFor(x => x.SessionId).GreaterThan(0);
        }
    }
}
